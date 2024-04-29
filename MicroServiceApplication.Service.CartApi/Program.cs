using MicroServiceApplication.Service.CartApi.Data;
using MicroServiceApplication.Service.CartApi.MappingProfile;
using MicroServiceApplication.Service.CartApi.Repository;
using MicroServiceApplication.Service.CartApi.Repository.IRepository;
using MicroServiceApplication.Service.CartApi.Service;
using MicroServiceApplication.Service.CartApi.Service.IService;
using MicroServiceApplication.Service.CartApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CartDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<BackEndApiAuthenticationHttpClintHandeler>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICouponService,CouponService>();
// Add services to the container.
builder.Services.AddHttpClient("Product", e => e.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackEndApiAuthenticationHttpClintHandeler>();
builder.Services.AddHttpClient("Coupon", e => e.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<BackEndApiAuthenticationHttpClintHandeler>();

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
