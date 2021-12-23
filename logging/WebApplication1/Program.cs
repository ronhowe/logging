using Serilog;
using Serilog.Events;

// Match this to appsettings.json for consistency.
const string outputTemplate = "**ConsoleTemplate** [{MachineName}] {Timestamp:HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console(outputTemplate: outputTemplate)
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

    if (app.Configuration.GetValue<bool>("MockException", false))
    {
        throw new NotImplementedException("**NotImplementedException()**");
    }

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
