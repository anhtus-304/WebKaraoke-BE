using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using WebKaraoke.Data;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.Business.Services;
using WebKaraoke.API.Middleware;
using WebKaraoke.Business.Helpers;
using WebKaraoke.Business.Mapping;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// 🟢 JSON options
// ---------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// ---------------------------
// 🟢 DbContext
// ---------------------------
builder.Services.AddDbContext<WebKaraokeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------
// 🟢 Business Services
// ---------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPhongService, PhongService>();
builder.Services.AddScoped<ILoaiPhongService, LoaiPhongService>(); // THÊM DÒNG NÀY
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDatPhongService, DatPhongService>();
builder.Services.AddScoped<IMonAnService, MonAnService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IKhuyenMaiService, KhuyenMaiService>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();

// ---------------------------
// 🟢 Helpers & AutoMapper
// ---------------------------
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// ---------------------------
// 🟢 JWT Authentication
// ---------------------------
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });

// ---------------------------
// 🟢 Authorization policies
// ---------------------------
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Khach", policy => policy.RequireRole("Khach"))
    .AddPolicy("NhanVien", policy => policy.RequireRole("NhanVien"))
    .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

// ---------------------------
// 🟢 Swagger + JWT
// ---------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "WebKaraoke API",
        Version = "v1",
        Description = "API for Karaoke Management System",
        Contact = new OpenApiContact { Name = "WebKaraoke Team" }
    });

    // JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ---------------------------
// 🟢 CORS (Fix lỗi Swagger "Failed to fetch")
// ---------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ---------------------------
// 🟢 Health check
// ---------------------------
builder.Services.AddHealthChecks();

var app = builder.Build();

// Development middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middleware pipeline
app.UseCors("AllowAll");
app.UseRouting();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebKaraoke API v1");
    c.RoutePrefix = "swagger"; // QUAN TRỌNG: Đặt route rõ ràng
    c.DocumentTitle = "WebKaraoke API Documentation";
});

// Endpoints
app.MapControllers();
app.MapHealthChecks("/health");

// 🟢 THÊM CÁC DÒNG NÀY - Default routes
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/index.html", () => Results.Redirect("/swagger"));

// Health check với thông tin chi tiết
app.MapGet("/api/status", () => new 
{
    Status = "Running",
    Timestamp = DateTime.UtcNow,
    Version = "1.0.0",
    Endpoints = new[] {
        "/swagger - API Documentation",
        "/health - Health Check", 
        "/api/phong - Room Management",
        "/api/loaiphong - Room Type Management"
    }
});

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WebKaraokeDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();