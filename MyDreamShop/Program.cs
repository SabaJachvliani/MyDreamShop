using Application.AuthUser;
using Application.Common.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.DBContext.MyDreamShopDb;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDreamShopDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(sp =>
    sp.GetRequiredService<MyDreamShopDbContext>());

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();