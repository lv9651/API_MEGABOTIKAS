using Microsoft.AspNetCore.Http.Features;
using SISLAB_API.Areas.Maestros.Services;

using Microsoft.Data.SqlClient;
using SISLAB_API.Areas.Maestros.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Kestrel (límite de archivos 3GB)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 3L * 1024 * 1024 * 1024;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 3L * 1024 * 1024 * 1024;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:3001", "https://vinaliclub.vinali.pe")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// 🔹 Inyección de dependencias (sin interfaces)
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<UsuarioServicio>();
builder.Services.AddScoped<UsuarioRepositorio>();



builder.Services.AddScoped<PuntajeRepositorio>();
builder.Services.AddScoped<PuntajeServicio>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar la política CORS (antes de Authorization)
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();