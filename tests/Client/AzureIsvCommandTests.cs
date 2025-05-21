// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using AzureMcp.Tests.Client.Helpers;
using Xunit;

namespace AzureMcp.Tests.Client;

public class AzureIsvCommandTests(McpClientFixture mcpClient, LiveTestSettingsFixture liveTestSettings, ITestOutputHelper output)
    : CommandTestsBase(mcpClient, liveTestSettings, output),
    IClassFixture<McpClientFixture>, IClassFixture<LiveTestSettingsFixture>
{
    [Fact]
    [Trait("Category", "Live")]
    public async Task Should_list_datadog_monitored_resources()
    {
        var result = await CallToolAsync(
            "azmcp-datadog-monitoredresources-list",
            new()
            {
                { "subscription", Settings.SubscriptionId },
                { "resource-group", Settings.ResourceGroupName },
                { "database-resource", Settings.ResourceBaseName }
            });

        var resources = result.AssertProperty("resources");
        if (Settings.TenantId != "70a036f6-8e4d-4615-bad6-149c02e7720d")
        {
            Assert.Skip("Test skipped because Tenant is not 'TME'.");
        }
        Assert.Equal(JsonValueKind.Array, resources.ValueKind);
        // Accept empty or non-empty, but must be an array
    }
}
