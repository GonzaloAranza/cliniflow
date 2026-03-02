using CliniFlow.API.Helper;
using Microsoft.OpenApi;
using CliniFlow.Application.Interfaces;
using CliniFlow.Application.Interfaces.Authentication;
using CliniFlow.Application.Services;
using CliniFlow.Infrastructure.Authentication;
using CliniFlow.Infrastructure.Data;
using CliniFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE BASE DE DATOS ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. CONFIGURACIÓN DE CORS (Preparando para el Frontend) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// --- 3. INYECCIÓN DE DEPENDENCIAS (Agrupada) ---
// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositorios
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();

// Servicios
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IProfessionalService, ProfessionalService>();

// --- 4. CONFIGURACIÓN JWT Y AUTENTICACIÓN ---
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

// --- 5. CONFIGURACIÓN DE CONTROLADORES Y SWAGGER ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CliniFlow API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorización JWT. Escribí 'Bearer' [espacio] y pegá tu token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // ESTA ES LA CLAVE: Forzamos el requerimiento mediante el filtro manual
    c.OperationFilter<AuthorizeCheckOperationFilter>();
});

var app = builder.Build();

// --- 6. MIDDLEWARES (Pipeline HTTP) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Activamos CORS
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();