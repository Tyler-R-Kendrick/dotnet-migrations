using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1812
internal class KernelSettings
{
    public const string DefaultConfigFile = "config/appsettings.json";

    [JsonPropertyName("endpointType")]
    public string EndpointType { get; set; } = EndpointTypes.ChatCompletion;

    [JsonPropertyName("serviceType")]
    public string ServiceType { get; set; } = string.Empty;

    [JsonPropertyName("serviceId")]
    public string ServiceId { get; set; } = string.Empty;

    [JsonPropertyName("deploymentOrModelId")]
    public string DeploymentOrModelId { get; set; } = string.Empty;

    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; } = string.Empty;

    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;

    [JsonPropertyName("orgId")]
    public string OrgId { get; set; } = string.Empty;

    [JsonPropertyName("logLevel")]
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// Load the kernel settings from settings.json if the file exists and if not attempt to use user secrets.
    /// </summary>
    internal static KernelSettings LoadSettings()
    {
        KernelSettings GetSettings(Func<IConfigurationBuilder, IConfigurationBuilder>? builder = null)
            => (builder ??= x => x)(new ConfigurationBuilder())
                .AddUserSecrets<KernelSettings>()
                .Build().Get<KernelSettings>()
                ?? throw new InvalidDataException($"Invalid semantic kernel settings in '{DefaultConfigFile}', please provide configuration settings using instructions in the README.");
  
        try
        {
            if (File.Exists(DefaultConfigFile))
            {
                return GetSettings(x => x
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile(DefaultConfigFile, optional: true, reloadOnChange: true));
            }
            //TODO: Ignore CA1303
            #pragma warning disable CA1303
            Console.WriteLine($"Semantic kernel settings '{DefaultConfigFile}' not found, attempting to load configuration from user secrets.");

            return GetSettings();
        }
        catch (InvalidDataException ide)
        {
            Console.Error.WriteLine(
                "Unable to load semantic kernel settings, please provide configuration settings using instructions in the README.\n" +
                "Please refer to: https://github.com/microsoft/semantic-kernel-starters/blob/main/sk-csharp-hello-world/README.md#configuring-the-starter"
            );
            throw new InvalidOperationException(ide.Message);
        }
    }
}
