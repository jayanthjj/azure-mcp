using AzureMcp.Services.Interfaces;
using Azure.ResourceManager.Datadog;
using System.Text.Json;
using Azure.Core;

namespace AzureMcp.Services.Azure.Datadog;

public partial class DatadogService : BaseAzureService, IDatadogService
{
    public DatadogService(ITenantService? tenantService = null) : base(tenantService)
    {
    }

    public async Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string tenant, string datadogResource)
    {
        // Resolve the tenant ID for the given subscription
        var tenantId = await ResolveTenantIdAsync(tenant);

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

    public Task<List<string>> CreateMonitor(string subscription, string monitorName, string resourceGroup, string location, string apiKey, string applicationKey)
    {
        Task.Delay(1000).Wait(); // Simulate some delay for the sake of example
        return Task.FromResult(new List<string> { subscription, monitorName, resourceGroup, location, apiKey, applicationKey });
        //var command = $"az datadog monitor create --name {monitorName} --resource-group {resourceGroup} --location {location} --datadog-organization-properties api-key={apiKey} application-key={applicationKey}";

        //var result = await _processService.ExecuteAsync("az", command, 300);

        //if (result.ExitCode != 0)
        //{
        //    throw new Exception($"Failed to create monitor: {result.Error}");
        //}
    }
}