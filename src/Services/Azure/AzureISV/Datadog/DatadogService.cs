using Azure.Core;
using Azure.ResourceManager.Datadog;
using AzureMcp.Arguments;
using AzureMcp.Models.AzureISV.Datadog;
using AzureMcp.Services.Interfaces;
using System.Text.Json;

namespace AzureMcp.Services.Azure.AzureISV.Datadog;

public partial class DatadogService( ISubscriptionService subscriptionService) : BaseAzureService, IDatadogService
{
    private readonly ISubscriptionService _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));

    public async Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string datadogResource)
    {
        try
        {
            var tenantId = await ResolveTenantIdAsync(null);
            var armClient = await CreateArmClientAsync(tenant: tenantId, retryPolicy: null);

            var resourceId = $"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.Datadog/monitors/{datadogResource}";

            ResourceIdentifier id = new ResourceIdentifier(resourceId);
            var datadogMonitorResource = armClient.GetDatadogMonitorResource(id);
            var monitoredResources = datadogMonitorResource.GetMonitoredResources();

            var resourceList = new List<string>();
            foreach (var resource in monitoredResources)
            {
                var resourceIdSegments = resource.Id.ToString().Split('/');
                var lastSegment = resourceIdSegments[^1];
                resourceList.Add(lastSegment);
            }

            return resourceList;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error listing monitored resources: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> ListDatadogResources(string subscription, RetryPolicyArguments? retryPolicy = null)
    {
        try
        {
            var tenantId = await ResolveTenantIdAsync(null);
            var armClient = await CreateArmClientAsync(tenant: tenantId, retryPolicy: null);

            var subscriptionResource = await _subscriptionService.GetSubscription(subscription, tenantId, retryPolicy);
            var datadogResources = new List<string>();

            try
            {
                await foreach (var datadogMonitor in subscriptionResource.GetDatadogMonitorResourcesAsync())
                {
                    if (datadogMonitor?.Data?.Name != null)
                    {
                        datadogResources.Add(datadogMonitor.Data.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Datadog resources: {ex.Message}", ex);
            }

            return datadogResources;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error listing Datadog resources: {ex.Message}", ex);
        }
    }

    public async Task<DatadogMonitorResourceModel> GetDatadogMonitorResourceData(string resourceGroup, string subscription, string datadogResource)
    {
        try
        {
            var tenantId = await ResolveTenantIdAsync(null);
            var armClient = await CreateArmClientAsync(tenant: tenantId, retryPolicy: null);

            var resourceId = $"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.Datadog/monitors/{datadogResource}";

            ResourceIdentifier id = new ResourceIdentifier(resourceId);
            var datadogMonitorResource = armClient.GetDatadogMonitorResource(id);

            var datadogMonitor = await datadogMonitorResource.GetAsync();

            if (datadogMonitor.Value.Data != null)
            {
                var _data = datadogMonitor.Value.Data;
                var model = new DatadogMonitorResourceModel
                {
                    Id = _data.Id?.ToString(),
                    Name = _data.Name,
                    Location = _data.Location.ToString(),
                    Tags = _data.Tags,
                    SkuName = _data.SkuName,
                    Properties = _data.Properties,
                };

                return model;
            }

            return new DatadogMonitorResourceModel();

        }
        catch (UriFormatException uriEx)
        {
            throw new Exception($"URI format error: {uriEx.Message}", uriEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving Datadog monitor resource data: {ex.Message}", ex);
        }
    }
}
