using MicroServiceWeb;
using MicroServiceWeb.Const;
using MicroServiceWeb.Service;
using MicroServiceWeb.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option =>
            {
                option.ExpireTimeSpan=TimeSpan.FromHours(3);
                option.LoginPath = "/User/LogIn";
                option.AccessDeniedPath = "/User/AccessDenied";
            });

SD.CouponApiBase = builder.Configuration["ServiceUrls:CouponAPI"];
SD.UserApiBase = builder.Configuration["ServiceUrls:UserAPI"];
SD.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.CartApi = builder.Configuration["ServiceUrls:CartApiAPI"];
SD.OrderApi = builder.Configuration["ServiceUrls:OrderApi"];
builder.Services.AddScoped<IBaseService,BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
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
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
