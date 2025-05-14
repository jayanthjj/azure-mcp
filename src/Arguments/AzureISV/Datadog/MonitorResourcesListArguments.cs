﻿using System.Text.Json.Serialization;
using AzureMcp.Models.Argument;

namespace AzureMcp.Arguments.AzureISV.Datadog;

public class MonitoredResourcesListArguments : SubscriptionArguments
{
    [JsonPropertyName(ArgumentDefinitions.Datadog.DatadogResourceName)]
    public string? DatadogResource { get; set; }
}
