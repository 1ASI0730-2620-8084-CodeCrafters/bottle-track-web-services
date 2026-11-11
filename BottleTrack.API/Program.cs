using System.Text.Json;
using System.Text.Json.Serialization;
using BottleTrack.API.Fleet.Application.Internal.CommandServices;
using BottleTrack.API.Fleet.Application.Internal.QueryServices;
using BottleTrack.API.Fleet.Application.Services;
using BottleTrack.API.Fleet.Domain.Repositories;
using BottleTrack.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BottleTrack.API.Iam.Application.Internal.CommandServices;
using BottleTrack.API.Iam.Application.Internal.QueryServices;
using BottleTrack.API.Iam.Application.Services;
using BottleTrack.API.Iam.Domain.Repositories;
using BottleTrack.API.Iam.Infrastructure.Hashing;
using BottleTrack.API.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BottleTrack.API.Incidents.Application.Internal.CommandServices;
using BottleTrack.API.Incidents.Application.Internal.QueryServices;
using BottleTrack.API.Incidents.Application.Services;
using BottleTrack.API.Incidents.Domain.Repositories;
using BottleTrack.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BottleTrack.API.Monitoring.Application.Internal.CommandServices;
using BottleTrack.API.Monitoring.Application.Internal.QueryServices;
using BottleTrack.API.Monitoring.Application.Services;
using BottleTrack.API.Monitoring.Domain.Repositories;
using BottleTrack.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BottleTrack.API.Operations.Application.Internal.CommandServices;
using BottleTrack.API.Operations.Application.Internal.QueryServices;
using BottleTrack.API.Operations.Application.Services;
using BottleTrack.API.Operations.Domain.Repositories;
using BottleTrack.API.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using BottleTrack.API.Resources;
using BottleTrack.API.Shared.Domain.Repositories;
using BottleTrack.API.Shared.Infrastructure.Interfaces.Asp.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using BottleTrack.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text;
using BottleTrack.API.Iam.Infrastructure.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();

builder.Services.AddRouting(options => options.LowercaseUrls = false);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower)))
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)))
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var localizer = context.HttpContext.RequestServices
                .GetRequiredService<IStringLocalizer<SharedResource>>();
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: StatusCodes.Status400BadRequest,
                title: localizer["InvalidModelStateTitle"].Value);

            var result = new BadRequestObjectResult(problemDetails);
            result.ContentTypes.Add("application/problem+json");
            return result;
        };
    });

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails.Status == null || context.ProblemDetails.Status >= 500)
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            if (context.ProblemDetails.Title == null)
                context.ProblemDetails.Title = localizer["UnexpectedServerError"].Value;

            if (context.ProblemDetails.Detail == null)
                context.ProblemDetails.Detail = localizer["UnexpectedErrorProcessingRequest"].Value;
        }
    };
});

builder.Services.AddOpenApi();

string[]? configuredOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
string[] allowedOrigins = configuredOrigins == null ? new string[0] : configuredOrigins;
builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrganizationCommandService, OrganizationCommandService>();
builder.Services.AddScoped<IOrganizationQueryService, OrganizationQueryService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleCommandService, VehicleCommandService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
builder.Services.AddScoped<IDriverCommandService, DriverCommandService>();
builder.Services.AddScoped<IDriverQueryService, DriverQueryService>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();

builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
builder.Services.AddScoped<IIncidentCommandService, IncidentCommandService>();
builder.Services.AddScoped<IIncidentQueryService, IncidentQueryService>();

builder.Services.AddScoped<IDeliveryPointRepository, DeliveryPointRepository>();
builder.Services.AddScoped<IDeliveryPointCommandService, DeliveryPointCommandService>();
builder.Services.AddScoped<IDeliveryPointQueryService, DeliveryPointQueryService>();
builder.Services.AddScoped<ITransportOperationRepository, TransportOperationRepository>();
builder.Services.AddScoped<ITransportOperationCommandService, TransportOperationCommandService>();
builder.Services.AddScoped<ITransportOperationQueryService, TransportOperationQueryService>();

builder.Services.AddScoped<IIotDeviceRepository, IotDeviceRepository>();
builder.Services.AddScoped<IIotDeviceCommandService, IotDeviceCommandService>();
builder.Services.AddScoped<IIotDeviceQueryService, IotDeviceQueryService>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<ISensorReadingCommandService, SensorReadingCommandService>();
builder.Services.AddScoped<ISensorReadingQueryService, SensorReadingQueryService>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();

JwtSettings? jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
if (jwtSettings == null)
    jwtSettings = new JwtSettings();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        TokenValidationParameters parameters = new TokenValidationParameters();
        parameters.ValidateIssuer = true;
        parameters.ValidIssuer = jwtSettings.Issuer;
        parameters.ValidateAudience = true;
        parameters.ValidAudience = jwtSettings.Audience;
        parameters.ValidateIssuerSigningKey = true;
        parameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        parameters.ValidateLifetime = true;
        options.TokenValidationParameters = parameters;
    });

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}

app.UseExceptionHandler();

string[] supportedCultures = new string[] { "en", "en-US", "es", "es-419" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "BottleTrack.API v1"));
app.MapControllers();

app.Run();

public partial class Program
{
}
