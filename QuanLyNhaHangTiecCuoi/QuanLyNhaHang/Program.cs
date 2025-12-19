using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google; // Thêm thư viện Google
using QuanLyNhaHang.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CẤU HÌNH DỊCH VỤ (SERVICES) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuanLyNhaHangContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Cache & Session (Giữ nguyên)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".NhaHang.Session";
});

// --- PHẦN SỬA ĐỔI: Tích hợp Google vào Authentication ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Ưu tiên Google khi có yêu cầu đăng nhập ngoài
})
    .AddCookie(options =>
    {
        options.LoginPath = "/DangNhap/Index";
        options.AccessDeniedPath = "/Home/TuChoiTruyCap";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    })
    .AddGoogle(options =>
    {
        // Lấy thông tin từ file appsettings.json mà bạn đã dán mã vào
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });
// -------------------------------------------------------

// Đăng ký các dịch vụ khác (Giữ nguyên)
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// --- 2. CẤU HÌNH MIDDLEWARE (PIPELINE) ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// THỨ TỰ QUAN TRỌNG: Session -> Routing -> Auth (Giữ nguyên thứ tự của bạn)
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();