using ChatBot.Hub;
using LoginService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var authSettings = builder.Configuration.GetSection("Authentication");

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authSettings["Authority"];
        options.Audience = authSettings["Audiance"];
        options.RequireHttpsMetadata = bool.Parse(authSettings["RequireHttpsMetadata"]!);
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var accessToken = ctx.Request.Query["access_token"];
                var path = ctx.HttpContext.Request.Path;
                if(!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    ctx.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            RoleClaimType = "role",
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("chatBot", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "chatBot");
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLoginService();

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
app.MapHub<ChatHub>("/chathub").RequireAuthorization();
app.UseCors();

app.Run();
