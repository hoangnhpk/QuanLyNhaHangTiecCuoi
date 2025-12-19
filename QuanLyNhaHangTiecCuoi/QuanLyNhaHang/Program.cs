using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google; // GIỮ: Thư viện Google chúng ta vừa thêm
using QuanLyNhaHang.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CẤU HÌNH DỊCH VỤ (SERVICES) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuanLyNhaHangContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Cache & Session (Gộp cài đặt của cả hai)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".NhaHang.Session";
});

// --- GIỮ: Tích hợp Google vào Authentication (Phần chúng ta vừa làm) ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/DangNhap/Index";
        options.AccessDeniedPath = "/Home/TuChoiTruyCap";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    })
    .AddGoogle(options =>
    {
        // Lấy thông tin từ file appsettings.json
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

// Đăng ký các dịch vụ khác
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();

// GIỮ: Đăng ký Controller & Fix lỗi JSON vòng lặp (Phần của người khác)
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

// --- GIỮ: Thứ tự Middleware kèm chú thích chi tiết của người khác ---

// 1. Kích hoạt Session ĐẦU TIÊN (để các bước sau có thể dùng dữ liệu session)
app.UseSession();

// 2. Sau đó mới đến định tuyến
app.UseRouting();

// 3. Cuối cùng là xác thực và phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();