using API.Data.Interfaces;
using API.Data.Realisations;
using API.Services;
using API.Services.Interfaces;
using API.Services.Realisations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using System.Text;

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
builder.Services.AddScoped<IEtatDAO, EtatSQLDAO>();
builder.Services.AddScoped<ITransitionDAO, TransitionSQLDAO>();
builder.Services.AddScoped<IAutomateDAO, AutomateSQLDAO>();

builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IUtilisateurDAO, UtilisateurSQLDAO>();
builder.Services.AddScoped<IHasherPassword, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, JWTokenService>();
builder.Services.AddScoped<IBDDConnection, SQLiteConnector>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
