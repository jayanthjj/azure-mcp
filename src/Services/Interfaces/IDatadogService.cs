namespace AzureMcp.Services.Interfaces;

public interface IDatadogService
{
    Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string databaseResource);
}