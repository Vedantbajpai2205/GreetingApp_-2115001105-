using BuisnessLayer.Interface;
using BuisnessLayer.Service;
using NLog;
using NLog.Config;
using NLog.Web;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL, GreetingRL>();

builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //logger using nlog
    var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    LogManager.Configuration = new XmlLoggingConfiguration("C:\\Users\\VEDANT SIR\\source\\repos\\HelloGreetingApplication\\Nlog.config");
    logger.Debug("init main");

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();//for UI colors
                       // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
