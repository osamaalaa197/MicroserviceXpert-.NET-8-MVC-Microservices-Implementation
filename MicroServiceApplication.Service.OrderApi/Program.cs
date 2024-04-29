using MicroServiceApplication.Service.OrderApi.Data;
using MicroServiceApplication.Service.OrderApi.IRepository;
using MicroServiceApplication.Service.OrderApi.Mapping;
using MicroServiceApplication.Service.OrderApi.Repository;
using MicroServiceApplication.Service.OrderApi.Service;
using MicroServiceApplication.Service.OrderApi.Service.IService;
using MicroServiceApplication.Service.OrderApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<OrderDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackEndApiAuthenticationHttpClintHandeler>();
builder.Services.AddHttpClient("Product", e => e.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackEndApiAuthenticationHttpClintHandeler>();
builder.Services.AddHttpClient("Cart", e => e.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartApiAPI"])).AddHttpMessageHandler<BackEndApiAuthenticationHttpClintHandeler>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();


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

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

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
