using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Text.Json.Serialization;
// 1. Thêm thư viện dùng Cookie Authentication
using Microsoft.AspNetCore.Authentication.Cookies;
using QuanLyNhaHang.Services;


var builder = WebApplication.CreateBuilder(args);
// --- BẮT ĐẦU ĐOẠN CẦN THÊM ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuanLyNhaHangContext>(options =>
    options.UseSqlServer(connectionString));
// --- CẤU HÌNH AUTHENTICATION ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Khi người dùng bấm vào trang có gắn thẻ [Authorize] mà chưa đăng nhập
        // Hệ thống sẽ tự động đá về đường dẫn này
        options.LoginPath = "/DangNhap/Index";

        // Khi người dùng đăng nhập rồi nhưng không đủ quyền (VD: Khách vào trang Admin)
        options.AccessDeniedPath = "/Home/TuChoiTruyCap";

        // Thời gian sống của Cookie (VD: 7 ngày)
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });


// --- Thêm dịch vụ Session ---
builder.Services.AddSession();
builder.Services.AddTransient<IEmailService, EmailService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
// Đăng ký Controller và Fix lỗi JSON bị vòng lặp ---
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- KÍCH HOẠT MIDDLEWARE  ---
app.UseAuthentication(); // <--- Phân quyền
app.UseAuthorization();  // <--- Quyền Hạn

app.UseSession();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
