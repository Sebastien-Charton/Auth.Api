using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Auth.Api.Web.Infrastructure;

public class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUri ?? throw new InvalidOperationException()))
                    {
                        IndexFormat =
                            $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1
                    })
                .Enrich.WithProperty("Environment",
                    context.HostingEnvironment.EnvironmentName ?? throw new InvalidOperationException())
                .Enrich.WithProperty("Application",
                    context.HostingEnvironment.ApplicationName ?? throw new InvalidOperationException())
                .ReadFrom.Configuration(context.Configuration);
        };
}
