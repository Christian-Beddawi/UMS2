using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using EmailServiceTools;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using UMS.Application.Common;
using UMS.Application.Entities.Course.Commands.InsertCourse;
using UMS.Domain.Models;
using UMS.Infrastructure.Abstraction.EmailServiceInterface;
using UMS.Infrastructure.EmailSendingService;
using UMS.Infrastructure.Settings;
using UMS.Persistence;
using UMS.WebAPI;
using UMS.WebAPI.NotificationHub;

//using ODataConventionModelBuilder = Microsoft.OData.Builder.ODataConventionModelBuilder;
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Role>("Roles");
    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

//Serilog and Elastic Search Configuration
ConfigureLogging();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https:/
// /aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program));
//builder.Services.AddScoped(typeof(umsContext),typeof(umsContext));
builder.Services.AddMediatR(typeof(InsertCourseCommand).GetTypeInfo().Assembly);

builder.Services.AddAutoMapper(typeof(Program));

//Add common DI
builder.Services.AddScoped<ICommonServices,CommonServices>();

//Multitenancy
/*
 //Multitenants

//builder.Services.Configure<TenantSetings>(builder.Configuration.GetSection(nameof(TenantSetings)));
//builder.Services.AddTransient<ITenantService, TenantService>();
IConfiguration config = builder.Configuration;
//builder.Services.AddAndMigrateTenantDatabases(config);

static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}
 
 */

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// KeyCLoak Configuration
builder.Services.AddMvc();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.Authority = builder.Configuration["https://localhost:8080/auth/realms/UMSRealm/UMS-WebApp"];
    o.Audience = builder.Configuration["UMS-WebApp"];
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            if (builder.Environment.IsDevelopment())
            {
                return c.Response.WriteAsync(c.Exception.ToString());
            }
            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});

//Email Configuration
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEEmailService,MailingService>();


//OData Configuration
//builder.Services.AddControllers().AddOData(opt => opt.Select().Filter().Expand().OrderBy().SetMaxTop(null).Count());
builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("v1", GetEdmModel()).Filter().Select().Expand().OrderBy().SetMaxTop(null).Count());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "UMS", Version = "v1" });
});

//connection string 
builder.Services.AddDbContext<umsContext>(
    options => options.UseNpgsql("Host=localhost;Port=5432;Database=ums;Username=postgres;Password=123456")
);

//SignalR
builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UMS v1"));
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

//SignalR
app.MapHub<NotificationHub>("/NotificationHub");

app.Run();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}