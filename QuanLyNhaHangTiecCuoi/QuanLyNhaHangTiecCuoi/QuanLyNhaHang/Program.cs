using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuanLyNhaHang.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CẤU HÌNH DỊCH VỤ (SERVICES) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuanLyNhaHangContext>(options =>
    options.UseSqlServer(connectionString));

// Cấu hình Cache & Session (CHỈ KHAI BÁO 1 LẦN DUY NHẤT Ở ĐÂY)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tăng lên 60 phút cho thoải mái
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Bắt buộc có dòng này
    options.Cookie.Name = ".NhaHang.Session"; // Đặt tên cho dễ quản lý
});

// Cấu hình Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/DangNhap/Index";
        options.AccessDeniedPath = "/Home/TuChoiTruyCap";

        // Thời gian sống của Cookie (VD: 7 ngày)
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// Đăng ký các dịch vụ khác
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddHttpContextAccessor();

// Đăng ký Controller & Fix lỗi JSON vòng lặp
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

// --- THỨ TỰ QUAN TRỌNG: Session -> Routing -> Auth ---

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