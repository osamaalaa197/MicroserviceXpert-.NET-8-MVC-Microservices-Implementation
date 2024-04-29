using AutoMapper;
using MicroServiceApplication.Service.CouponAPI.Data;
using MicroServiceApplication.Service.CouponAPI.IRepository;
using MicroServiceApplication.Service.CouponAPI.Mapping;
using MicroServiceApplication.Service.CouponAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});



builder.Services.AddScoped<ICouponRepository,CouponRepository>();
// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var Issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
var Audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
var secret = Encoding.ASCII.GetBytes(key);
builder.Services.AddAuthentication(e =>
{
    e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(e =>
{
    e.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = Issuer,
        ValidAudience = Audience,
    };
});
builder.Services.AddAuthorization();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
