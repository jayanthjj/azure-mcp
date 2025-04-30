using AzureMcp.Services.Interfaces;
using System.Text.Json;

namespace AzureMcp.Services.Azure.Datadog;

public partial class DatadogService : IDatadogService
{
    private readonly IExternalProcessService _processService;

    public DatadogService(IExternalProcessService processService)
    {
        _processService = processService;
    }

    public async Task<List<string>> ListMonitoredResources(string resourceGroup, string subscription, string databaseResource)
    {
        var command = $"az datadog monitor list-monitored-resource --name {databaseResource} --resource-group {resourceGroup}";

        var result = await _processService.ExecuteAsync("az", command, 300);

        if (result.ExitCode != 0)
        {
            throw new Exception($"Failed to fetch monitored resources: {result.Error}");
        }

        var resources = JsonSerializer.Deserialize<List<string>>(result.Output);
        return resources ?? new List<string>();
    }

    public async Task<List<string>> CreateMonitor(string subscription, string monitorName, string resourceGroup, string location, string apiKey, string applicationKey)
    {
        Task.Delay(1000).Wait(); // Simulate some delay for the sake of example
        return new List<string> { subscription, monitorName, resourceGroup, location, apiKey, applicationKey };
        //var command = $"az datadog monitor create --name {monitorName} --resource-group {resourceGroup} --location {location} --datadog-organization-properties api-key={apiKey} application-key={applicationKey}";

        //var result = await _processService.ExecuteAsync("az", command, 300);

        //if (result.ExitCode != 0)
        //{
        //    throw new Exception($"Failed to create monitor: {result.Error}");
        //}
    }
}