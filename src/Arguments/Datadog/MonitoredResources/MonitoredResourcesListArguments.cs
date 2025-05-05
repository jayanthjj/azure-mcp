using AzureMcp.Models.Argument;
using System.Text.Json.Serialization;

namespace AzureMcp.Arguments.Datadog.MonitoredResources;

public class MonitoredResourcesListArguments : SubscriptionArguments
{
    [JsonPropertyName(ArgumentDefinitions.Datadog.DatadogResourceName)]
    public string? DatadogResource { get; set; }
}