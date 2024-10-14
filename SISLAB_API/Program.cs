using SISLAB_API.Areas.Maestros.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la ruta compartida de la configuración
var sharedPath = builder.Configuration["SharedPath"];

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3001")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

// Repositorios
builder.Services.AddScoped<EmpleadoRepository>();
builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<NoticiaRepository>();
builder.Services.AddScoped<DocumentRepository>();
builder.Services.AddScoped<InductionRepository>();
builder.Services.AddScoped<UserRepository>();


// Servicios
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<NoticiaService>();
builder.Services.AddScoped<InductionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DataTransferService>();
// Registrar DocumentoService con la ruta compartida
builder.Services.AddScoped<DocumentoService>(provider =>
    new DocumentoService(
        provider.GetRequiredService<DocumentRepository>(),
        sharedPath)); // Pasar la ruta compartida

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar la política CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();