using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddOcelot();
var app = builder.Build();
app.UseOcelot();
app.MapGet("/", () => "Hello World!");

app.Run();
