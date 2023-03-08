using System.Text;
using FluxoDeCaixa.API;
using FluxoDeCaixa.Repositorio.Entidades;
using FluxoDeCaixa.Repositorio.Repositorios;
using FluxoDeCaixa.Service.Entidades;
using FluxoDeCaixa.Service.Interfaces;
using FluxoDeCaixa.Service.Servicos;
using LiteDB;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Events;


using Serilog.Formatting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(new ColoredJsonFormatter())
);


// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();

// define os endpoints usando a abordagem Minimal API
Endpoints.MapEndpoints(app);

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


// Use Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value, false);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme, false);
        diagnosticContext.Set("RequestMethod", httpContext.Request.Method, false);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path, false);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"], false);
        diagnosticContext.Set("Accept", httpContext.Request.Headers["Accept"], false);
        diagnosticContext.Set("AcceptEncoding", httpContext.Request.Headers["Accept-Encoding"], false);
        diagnosticContext.Set("ContentLength", httpContext.Request.ContentLength ?? 0, false);
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier, false);
    };
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        if (ex != null)
        {
            LogContext.PushProperty("RequestBody", GetRequestBody(httpContext));
            return LogEventLevel.Error;
        }
        if (httpContext.Response.StatusCode >= 500)
        {
            return LogEventLevel.Error;
        }
        if (TimeSpan.FromSeconds(elapsed) > TimeSpan.FromSeconds(1))
        {
            return LogEventLevel.Warning;
        }
        return LogEventLevel.Information;
    };
});

app.UseMiddleware<MiddlewareExceptionHandler>();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<ILancamentosRepositorio, LancamentosRepositorio>();
    services.AddScoped<ILancamentosServico, LancamentosServico>();
    services.AddSingleton<LiteDatabase>(_ =>
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var parentDirectory = Directory.GetParent(currentDirectory)?.FullName.Replace("src", "");
        var databasePath = Path.Combine(parentDirectory ?? string.Empty, "database", "lancamentos.db");

        return new LiteDatabase(databasePath);
    });


    services.AddAutoMapper(config =>
    {
        config.CreateMap<Lancamento, LancamentoLiteDb>().ReverseMap();
    }, typeof(Program).Assembly);

}



app.Run();

string GetRequestBody(HttpContext httpContext)
{
    var body = "";
    if (httpContext.Request.Body.CanSeek)
    {
        httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true);
        body = reader.ReadToEnd();
    }
    return body;
}


public class ColoredJsonFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var message = logEvent.RenderMessage();
        var properties = logEvent.Properties
            .Where(p => p.Value != null && p.Value.GetType() == typeof(ScalarValue) && p.Value.ToString().StartsWith("\"") && p.Value.ToString().EndsWith("\""))
            .ToDictionary(p => p.Key, p => p.Value.ToDecentString());

        var log = new
        {
            Timestamp = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"),
            Level = logEvent.Level.ToString(),
            Message = message,
            Exception = logEvent.Exception?.ToString(),
            Properties = properties
        };

        var serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        var json = JsonConvert.SerializeObject(log, serializerSettings);

        var levelColor = GetLevelColor(logEvent.Level);
        var levelText = $"\x1b[38;5;{levelColor}m{log.Level}\x1b[0m";

        output.WriteLine($"[{log.Timestamp} {levelText}] {log.Message}");
        output.WriteLine(json);
    }

    private int GetLevelColor(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => 251,
            LogEventLevel.Debug => 250,
            LogEventLevel.Information => 45,
            LogEventLevel.Warning => 226,
            LogEventLevel.Error => 196,
            LogEventLevel.Fatal => 199,
            _ => 15
        };
    }
}

public static class LogEventPropertyValueExtensions
{
    public static string ToDecentString(this LogEventPropertyValue value)
    {
        using var stringWriter = new StringWriter();
        value.Render(stringWriter);
        return stringWriter.ToString();
    }
}

