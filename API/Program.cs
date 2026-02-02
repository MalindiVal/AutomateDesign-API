using API.Data.Interfaces;
using API.Data.Realisations;
using API.Services;
using API.Services.Interfaces;
using API.Services.Realisations;
using SQLitePCL;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title =
   "AutomateAPI",
        Version = "v1"
    });
    var filePath = "AutomateAPIDocumentation.xml";
    c.IncludeXmlComments(filePath);
});

builder.Services.AddScoped<IAutomateService, AutomateService>();
builder.Services.AddScoped<IAutomateDAO, AutomateDAO>();

builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IUtilisateurDAO, UtilisateurDAO>();
builder.Services.AddScoped<IHasherPassword, BCryptPasswordHasher>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
