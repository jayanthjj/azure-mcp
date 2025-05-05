using Azure.Core;
using Azure.ResourceManager.Datadog;
using AzureMcp.Services.Interfaces;
using System.Text.Json;

namespace AzureMcp.Services.Azure.Datadog;

public partial class DatadogService : BaseAzureService, IDatadogService
{
    public DatadogService(ITenantService? tenantService = null) : base(tenantService)
    {
    }

    public async Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string datadogResource)
    {
        // Resolve the tenant ID for the given subscription
        var tenantId = await ResolveTenantIdAsync(null);

        // Create an authenticated client for the DatadogMonitorResource
        var armClient = await CreateArmClientAsync(tenant: tenantId, retryPolicy: null);

        // Get the Datadog monitor resource
        var resourceId = $"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.Datadog/monitors/{datadogResource}";

        ResourceIdentifier id = new ResourceIdentifier(resourceId);
        var datadogMonitorResource = armClient.GetDatadogMonitorResource(id);

        // Fetch monitored resources
        var monitoredResources = datadogMonitorResource.GetMonitoredResources();

        // Convert the results to a list of strings
        var resourceList = new List<string>();
        foreach (var resource in monitoredResources)
        {
            // Extract the last segment of the resource ID
            var resourceIdSegments = resource.Id.ToString().Split('/');
            var lastSegment = resourceIdSegments[^1]; // Using ^1 to get the last element
            resourceList.Add(lastSegment);
        }

        return resourceList;
    }

}