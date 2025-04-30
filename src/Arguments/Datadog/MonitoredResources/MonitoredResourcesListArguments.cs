namespace AzureMcp.Arguments.Datadog.MonitoredResources;

public class MonitoredResourcesListArguments : SubscriptionArguments
{
    public string? DatadogResource { get; set; }
}