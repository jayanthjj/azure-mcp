// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;
using System.Text.Json.Serialization;
using AzureMcp.Arguments.Datadog.MonitoredResources;
using AzureMcp.Commands.Datadog.MonitoredResources;
using AzureMcp.Models.Command;
using AzureMcp.Models.Datadog;
using AzureMcp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AzureMcp.Tests.Commands.Datadog.MonitoredResources;

public class DatadogResourceDetailsCommandTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDatadogService _datadogService;
    private readonly ILogger<DatadogResourceDetailsCommand> _logger;

    public DatadogResourceDetailsCommandTests()
    {
        _datadogService = Substitute.For<IDatadogService>();
        _logger = Substitute.For<ILogger<DatadogResourceDetailsCommand>>();

        var collection = new ServiceCollection();
        collection.AddSingleton(_datadogService);

        _serviceProvider = collection.BuildServiceProvider();
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsResourceDetails_WhenResourceExists()
    {
        // Arrange
        var expectedResource = new DatadogMonitorResourceModel
        {
            Id = "/subscriptions/1234/resourceGroups/rg-demo/providers/Microsoft.Datadog/monitors/app-demo-1",
            Name = "app-demo-1",
            Location = "eastus"
        };

        _datadogService.GetDatadogMonitorResourceData("rg1", "sub123", "datadog1")
            .Returns(expectedResource);

        var command = new DatadogResourceDetailsCommand(_logger);
        var args = command.GetCommand().Parse("--subscription sub123 --resource-group rg1 --datadog-resource datadog1");
        var context = new CommandContext(_serviceProvider);

        // Act
        var response = await command.ExecuteAsync(context, args);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Results);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsNull_WhenResourceDoesNotExist()
    {
        // Arrange
        _datadogService.GetDatadogMonitorResourceData("rg1", "sub123", "datadog1")
            .Returns(new DatadogMonitorResourceModel());

        var command = new DatadogResourceDetailsCommand(_logger);
        var args = command.GetCommand().Parse("--subscription sub123 --resource-group rg1 --datadog-resource datadog1");
        var context = new CommandContext(_serviceProvider);

        // Act
        var response = await command.ExecuteAsync(context, args);

        // Assert
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ExecuteAsync_HandlesException()
    {
        // Arrange
        var expectedError = "An error occurred while fetching resource details.";
        _datadogService.GetDatadogMonitorResourceData("rg1", "sub123", "datadog1")
            .ThrowsAsync(new Exception(expectedError));

        var command = new DatadogResourceDetailsCommand(_logger);
        var args = command.GetCommand().Parse("--subscription sub123 --resource-group rg1 --datadog-resource datadog1");
        var context = new CommandContext(_serviceProvider);

        // Act
        var response = await command.ExecuteAsync(context, args);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(500, response.Status);
        Assert.StartsWith(expectedError, response.Message);
    }

    private class DatadogMonitorResourcesResult
    {
        [JsonPropertyName("resource")]
        public DatadogMonitorResourceModel Resources { get; set; } = new();
    }
}
