using System.Text.Json;
using Azure.Core;
using Azure.ResourceManager.Datadog;
using AzureMcp.Models.Datadog;
using AzureMcp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Services.Azure.Datadog;

public partial class DatadogService : BaseAzureService, IDatadogService
{
    public DatadogService(ITenantService? tenantService = null) : base(tenantService)
    {
    }

    public async Task<List<DatadogMonitoredResource>> ListMonitoredResources(string resourceGroup, string subscription, string datadogResource)
    {
        try
        {
            var tenantId = await ResolveTenantIdAsync(null);
            var armClient = await CreateArmClientAsync(tenant: tenantId, retryPolicy: null);

            var resourceId = $"/subscriptions/{subscription}/resourceGroups/{resourceGroup}/providers/Microsoft.Datadog/monitors/{datadogResource}";

            ResourceIdentifier id = new ResourceIdentifier(resourceId);
            var datadogMonitorResource = armClient.GetDatadogMonitorResource(id);
            var monitoredResourcesRaw = datadogMonitorResource.GetMonitoredResources();

            var monitoredResources = monitoredResourcesRaw.Select(resource => new DatadogMonitoredResource
            {
                Id = resource.Id?.ToString(),
                SendingMetrics = resource.SendingMetrics,
                ReasonForMetricsStatus = resource.ReasonForMetricsStatus,
                SendingLogs = resource.SendingLogs,
                ReasonForLogsStatus = resource.ReasonForLogsStatus
            }).Take(25).ToList();

            return monitoredResources;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error listing monitored resources: {ex.Message}", ex);
        }
    }
}
