using Basket.API.RabbitMQ;
using Basket.API.RabbitMQ.Publisher;
using Basket.API.RabbitMQ.Publisher.Interface;
using Basket.API.Services.LoginService;
using Basket.API.Services.ProductService;
using Caching.Redis;
using Ecommerce.Base;
using EventStore.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IProductService, ProductService>(u => u.BaseAddress =
              new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.Audience = "basketApi";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            RoleClaimType = "role",
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("basketApi", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "basketApi");
    });

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
/*builder.Services.AddSingleton(
    new EventStoreClient(EventStoreClientSettings.Create(
            "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false")));*/
builder.Services.AddRedisCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
