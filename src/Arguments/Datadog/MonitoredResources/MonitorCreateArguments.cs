using AzureMcp.Models.Argument;
using System.Text.Json.Serialization;

namespace AzureMcp.Arguments.Datadog.MonitoredResources;

public class MonitorCreateArguments : SubscriptionArguments
{
    [JsonPropertyName(ArgumentDefinitions.Datadog.MonitorResourceName)]
    public string? MonitorName { get; set; }

    [JsonPropertyName(ArgumentDefinitions.Datadog.LocationName)]

    public string? Location { get; set; }
   
     [JsonPropertyName(ArgumentDefinitions.Datadog.ApiKeyName)]

    public string? ApiKey { get; set; }
   
    [JsonPropertyName(ArgumentDefinitions.Datadog.ApplicationKeyName)]

    public string? ApplicationKey { get; set; }
}