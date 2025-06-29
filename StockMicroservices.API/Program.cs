using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using MassTransit.AspNetCoreIntegration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StockMicroservices.API.Data;
using StockMicroservices.API.Models;
using StockMicroservices.API.Repository;
using StockMicroservices.API.Services;
using StockMicroservices.API.EventBusConsumer;
using StockMicroservices.API.Models.Daos;
using StockMicroservices.EventBus.Common;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StockMicroservices.API.Errors;
using System.Net;
using OpenTelemetry.Logs;
using StockMicroservices.API.Utils;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Enum as strings
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // CamelCase naming
    options.JsonSerializerOptions.WriteIndented = true; // Indented JSON (optional)
});


string identityServerUrl = builder.Configuration.GetValue(typeof(string), "IdentityServerUrl").ToString();
string apiScope = builder.Configuration.GetValue(typeof(string), "APIScope").ToString();
string otlpEndpoint = builder.Configuration.GetValue(typeof(string), "OTEL_EXPORTER_OTLP_ENDPOINT").ToString();
string serviceName = builder.Configuration.GetValue(typeof(string), "OTEL_SERVICE_NAME").ToString();
if (string.IsNullOrEmpty(serviceName))
{
    serviceName = "StockAPI";
}
Instrumentation.ServiceName = serviceName;
builder.Services.AddSingleton<Instrumentation>();

// OpenTelemetry Resource (used by logs, metrics, and traces)
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: serviceName);
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.AddConsoleExporter();
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
    options.AddOtlpExporter(options =>
    {
        if (!string.IsNullOrEmpty(otlpEndpoint))
        {
            options.Endpoint = new Uri($"{otlpEndpoint}/v1/logs");
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        }
    });
});
var otel = builder.Services.AddOpenTelemetry().ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: "1.0.1"))
      .WithTracing(tracing =>
          tracing
          .AddSource(serviceName)
          .SetSampler(new AlwaysOnSampler())
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddConsoleExporter()
          .AddOtlpExporter(options =>
          {
              if (!string.IsNullOrEmpty(otlpEndpoint))
              {
                  options.Endpoint = new Uri($"{otlpEndpoint}/v1/traces");
                  options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
              }
          }))
      .WithMetrics(metrics => metrics
            .AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http"
          )
          .AddRuntimeInstrumentation()
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddConsoleExporter()
          .AddOtlpExporter(options =>
          {
              if (!string.IsNullOrEmpty(otlpEndpoint))
              {
                  options.Endpoint = new Uri($"{otlpEndpoint}/v1/metrics");
                  options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
              }
          }));

//Database
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StockDbContext>(options => options.UseInMemoryDatabase("InMemoryDbFor"));

//To run Stock.API.Test comeent out lines 47 - 64 and comment out the Authorization headers on the controllers
builder.Services.AddAuthentication(options => options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("Bearer",
                      config =>
                      {
                          config.Authority = identityServerUrl;
                          config.Audience = apiScope;
                          config.RequireHttpsMetadata = false;
                          config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                          {
                              ValidateAudience = false,
                              ValidateIssuer = false
                          };
                      });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StockAPIPolicy",
                      policy =>
                      {
                          policy.AuthenticationSchemes.Add("Bearer");
                          policy.AddRequirements(new StockMicroservices.API.Authorization.StockAPIRequirement(apiScope));
                      });

});

builder.Services.AddScoped<IAuthorizationHandler, StockMicroservices.API.Authorization.StockAPIRequirementHandler>();
builder.Services.AddCors(config =>
{
    config.AddPolicy("AllowAll",
                     p =>
                     {
                         p.AllowAnyOrigin();
                         p.AllowAnyHeader();
                         p.AllowAnyMethod();

                     });
});


//AutoMapper
var mappingConfiguration = new MapperConfiguration(config =>
{
    config.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockAPI", Version = "v1" });
});

string _hostname = builder.Configuration["RabbitMq:Hostname"];
string _username = builder.Configuration["RabbitMq:Username"];
string _password = builder.Configuration["RabbitMq:Password"];
string hostAddress = string.Format("amqp://{0}:{1}@{2}:5672", _username, _password, _hostname);

//MassTransit-RabbitMq
if (!builder.Environment.IsEnvironment("test"))
{
    builder.Services.AddMassTransit(config =>
    {
        config.AddConsumer<StockUpdateConsumer>();

        config.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(hostAddress);

            cfg.ReceiveEndpoint(EventBusConstants.STOCK_UPDATE_QUEUE, ep =>
            {
                ep.ConfigureConsumer<StockUpdateConsumer>(provider);
            });
        }));
    });
    //builder.Services.AddMassTransitHostedService();
    builder.Services.AddSingleton<IHostedService, MassTransitHostedService>();
}

//StockMarketService
builder.Services.AddScoped<IStockMarketService, StockMarketService>();

//Repositories
builder.Services.AddScoped<IRepository<StockOrder>, StockOrderRepository>();
builder.Services.AddScoped<IRepository<StockPosition>, StockPositionRepository>();
builder.Services.AddScoped<IRepository<Stock>, StockRepository>();
builder.Services.AddScoped<IRepository<StockHistory>, StockHistoryRepository>();
builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection("Database"));
builder.Services.AddHealthChecks()
   .AddCheck("self", () => HealthCheckResult.Healthy());

var app = builder.Build();

if (builder.Environment.IsEnvironment("local") || builder.Environment.IsEnvironment("development"))
{
    using var scope = app.Services.CreateScope();
    var stockDbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    SeedData.InitializeDB(stockDbContext);
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockAPI v1");
});



app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            var message = (contextFeature.Error != null) ? contextFeature.Error.Message : "Internal Server Error (500)";
            if (contextFeature.Endpoint != null)
            {
                message += String.Format(" {0}", contextFeature.Endpoint);
            }

            var errorMessage = new ErrorMessage
            {
                Message = message,
                StackTrace = (contextFeature.Error != null) ? contextFeature.Error.StackTrace : string.Empty
            };

            var jsonString = JsonConvert.SerializeObject(errorMessage);

            await context.Response.WriteAsync(jsonString);
        }
    });
});

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

_ = app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    _ = endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("self")
    });
});

_ = app.Services.GetRequiredService<TracerProvider>();

await app.RunAsync();
