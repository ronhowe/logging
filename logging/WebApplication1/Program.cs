using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "**ConsoleTemplate** {Timestamp:HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
    .CreateLogger();

Log.Debug("**Log.Debug()**");
Log.Information("**Log.Information()**");
Log.ForContext("SourceContext", "**SourceContext**").Information("**Log.ForContext().Information()**");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((hostContext, LoggerConfiguration) =>
    {
        LoggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapGet("/", () =>
    {
        app.Logger.LogDebug("**app.Logger.LogDebug()**");
        app.Logger.LogInformation("**app.Logger.LogInformation()**");
    });
    throw new NotImplementedException("DIE");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "**Log.Fatal()**");
}
finally
{
    Log.CloseAndFlush();
}
