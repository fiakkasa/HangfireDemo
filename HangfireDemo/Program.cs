using HangfireDemo.Extensions;
using HangfireDemo.Interfaces;
using HangfireDemo.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
    options.MapTypeSpan()
);

// demo service to show case work in a hangfire context
services.AddScoped<IProcessText, ProcessTextService>();

services.AddHangfireScheduler(configuration);

services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHangfireScheduler();

app.MapControllers();

app.Run();
