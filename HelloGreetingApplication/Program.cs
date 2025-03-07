using BuisnessLayer.Interface;
using BusinessLayer.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Config;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");//used for connection to database
builder.Services.AddDbContext<GreetingContext>(options => options.UseSqlServer(connectionString));
var ConnectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(ConnectionString));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL, GreetingRL>();
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserRL, UserRL>();

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
