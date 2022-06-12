using Microsoft.AspNetCore.Cors.Infrastructure;
using Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy(
        nameof(CorsPolicy),
        p => p.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
    ));

await MeshService.Initilize();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(nameof(CorsPolicy));
app.MapControllers();

app.Run();
