using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Interfaces;
//using AuthService.Application.Services;       // Bu namespace içinde JwtTokenGenerator implementasyonunu yerleþtireceðiz


using AuthService.Infrastructure.Services;// JwtTokenGenerator’ý buraya koyacaðýz
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using MediatR.Extensions.Microsoft.DependencyInjection;

using Microsoft.IdentityModel.Tokens;
using MediatR;
using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1) Connection string’i al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2) DbContext kayýt
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3) Identity kayýt (ApplicationUser, ApplicationRole)
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();


// 4) JWT Konfigürasyonu
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// 5) MediatR ve IJwtTokenGenerator
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.Load("AuthService.Application")));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

// 6) Controller, FluentValidation ve Swagger
builder.Services.AddControllers()
    .AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssembly(Assembly.Load("AuthService.Application"));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();