using System.Collections.Generic;
using Azure.ResourceManager.Datadog.Models;

namespace AzureMcp.Models.Datadog
{
    public class DatadogMonitorResourceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        public string SkuName { get; set; }
        public MonitorProperties Properties { get; set; }
    }
}