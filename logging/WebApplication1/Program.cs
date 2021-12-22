using Serilog;

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
        app.Logger.LogInformation("**MapGet**");
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "WebApplication1 Failed");
}
finally
{
    Log.CloseAndFlush();
}
