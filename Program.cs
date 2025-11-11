using ITSupportBE.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// === 1. ĐỊNH NGHĨA CHUỖI CORS ===
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// === 2. THÊM SERVICES ===
builder.Services.AddControllers()
    // Cấu hình này để tránh lỗi "object cycle" khi JOIN (rất quan trọng)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Thêm CORS Service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173") // Cho phép Vue app
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Lấy chuỗi kết nối
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Đăng ký DbContext
builder.Services.AddDbContext<ITSupportDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === 3. CẤU HÌNH HTTP PIPELINE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === 4. SỬ DỤNG CORS ===
app.UseCors(MyAllowSpecificOrigins);

//app.UseAuthorization(); // (chưa dùng [Authorize] nên dòng này không cần)

app.MapControllers();

app.Run();