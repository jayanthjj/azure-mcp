
using System.Text.Json.Serialization;
using AzureMcp.Commands.Datadog.MonitoredResources;
using AzureMcp.Commands.Monitor.Workspace;

namespace AzureMcp.Commands.Datadog;

[JsonSerializable(typeof(MonitoredResourcesListCommand.MonitoredResourcesListResult))]
[JsonSerializable(typeof(DatadogResourceDetailsCommand.DatadogMonitorResourcesResult))]

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class DatadogJsonContext : JsonSerializerContext
{
}
