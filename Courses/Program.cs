using Application;
using Application.Behaviors;
using Application.Common;
using Courses.Exceptions;
using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMediatR(options =>
    options.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(IAssemblyMarker).Assembly);

builder.Services.AddDbContext<Context>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowOrigins",
        policy  =>
        {
            policy.WithOrigins("*");
        });
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IContext, Context>();
builder.Services.AddSwaggerGen();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();