using System.Text.Json.Serialization;
using AzureMcp.Models.Argument;

namespace AzureMcp.Arguments.Datadog.MonitoredResources;

public class MonitoredResourcesListArguments : SubscriptionArguments
{
    [JsonPropertyName(ArgumentDefinitions.Datadog.DatadogResourceName)]
    public string? DatadogResource { get; set; }
}
