
using MicroServiceApplication.Service.ProductAPI.Data;
using MicroServiceApplication.Service.ProductAPI.MappingProfile;
using MicroServiceApplication.Service.ProductAPI.Repository;
using MicroServiceApplication.Service.ProductAPI.Repository.IRepository;
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

#region ConnectionDataBase
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductContext>(option =>
{
    option.UseSqlServer(connectionString);
});
#endregion


builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddScoped<IProudctRepository,ProudctRepository>();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();	
app.MapControllers();

app.Run();
