using System.Collections.Generic;
using Azure.ResourceManager.Elastic.Models;

namespace AzureMcp.Models.AzureISV.Datadog
{
    public class ElasticMonitorResourceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IDictionary<string, string> Tags { get; set; }
        public string SkuName { get; set; }
        public ElasticMonitorProperties Properties { get; set; }
    }
}
