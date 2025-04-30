namespace AzureMcp.Services.Interfaces;

public interface IDatadogService
{
    Task<List<string>> CreateMonitor(string subscription, string monitorName, string resourceGroup, string location, string apiKey, string applicationKey);
    Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string databaseResource);
}