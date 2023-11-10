using MinimalApi.Infrastructure.Middleware;
using NLog;
using NLog.Web;
using PlanetApi.Infrastructure.Extensions;
using PlanetApi.Infrastructure.Logger;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Info("Starting web application");

    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<ILoggerService, LoggerService>();

    //builder.Services.ConfigureLogic();
    builder.Services.ConfigureFactories();
    builder.Services.ConfigureRepositories();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<ExceptionHandler>();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    LogManager.Shutdown();
}
