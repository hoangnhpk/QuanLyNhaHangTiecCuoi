using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Models;
using System.Text.Json.Serialization;

using QuanLyNhaHang.Services;


var builder = WebApplication.CreateBuilder(args);
// --- BẮT ĐẦU ĐOẠN CẦN THÊM ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuanLyNhaHangContext>(options =>
    options.UseSqlServer(connectionString));


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

app.UseAuthorization();
// Kích hoạt sử dụng Session trong ứng dụng
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
