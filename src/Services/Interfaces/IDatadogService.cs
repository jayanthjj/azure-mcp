using AzureMcp.Models.Datadog;

namespace AzureMcp.Services.Interfaces;

public interface IDatadogService
{
    Task<List<DatadogMonitoredResource>> ListMonitoredResources(string resourceGroup, string subscription, string datadogResource);
    Task<DatadogMonitorResourceModel> GetDatadogMonitorResourceData(string resourceGroup, string subscription, string datadogResource);
}
