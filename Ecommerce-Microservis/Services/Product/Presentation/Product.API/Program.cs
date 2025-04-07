using Ecommerce.Base;
using Ecommerce.Base.Repositories;
using Ecommerce.Base.Repositories.Interface;
using Product.Application;
using Product.Application.Features.Command.CreateProduct;
using Product.Application.Features.Command.DeleteProduct;
using Product.Application.Features.Command.UpdateProduct;
using Product.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddBaseServices();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
