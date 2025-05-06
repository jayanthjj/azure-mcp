using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureMcp.Arguments.Datadog.MonitoredResources;
using AzureMcp.Models.Argument;
using AzureMcp.Models.Command;
using AzureMcp.Models.Datadog;
using AzureMcp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AzureMcp.Commands.Datadog.MonitoredResources;

public sealed class DatadogResourceDetailsCommand(ILogger<DatadogResourceDetailsCommand> logger) : SubscriptionCommand<DatadogResourceDetailsArguments>()
{
    private readonly ILogger<DatadogResourceDetailsCommand> _logger = logger;

    protected override string GetCommandName() => "details";

    protected override string GetCommandDescription() =>
    $"""
    Get the details of the specific datadog resources provided by the user.
       Required arguments:
       - subscription: The name of the Azure subscription
       - resource-group: The name of the Azure resource group
       - datadog-resource: The name of the Datadog resource
    """;

    private readonly Option<string> _datadogResourceOption = ArgumentDefinitions.Datadog.DatadogResource.ToOption();

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_datadogResourceOption);
        command.AddOption(_resourceGroupOption);
    }
    protected override void RegisterArguments()
    {
        base.RegisterArguments();
        AddArgument(CreateDatadogResourceArgument());
        AddArgument(CreateResourceGroupArgument());
    }

    protected override DatadogResourceDetailsArguments BindArguments(ParseResult parseResult)
    {
        var args = base.BindArguments(parseResult);
        args.DatadogResource = parseResult.GetValueForOption(_datadogResourceOption);
        args.ResourceGroup = parseResult.GetValueForOption(_resourceGroupOption);
        return args;
    }

    public override async Task<CommandResponse> ExecuteAsync(CommandContext context, ParseResult parseResult)
    {
        var args = BindArguments(parseResult);

        try
        {
            if (!await ProcessArguments(context, args))
            {
                return context.Response;
            }

            var service = context.GetService<IDatadogService>();
            DatadogMonitorResourceModel results = await service.GetDatadogMonitorResourceData(
                args.ResourceGroup!,
                args.Subscription!,
                args.DatadogResource!);

            context.Response.Results = results != null ?
                ResponseResult.Create(new DatadogMonitorResourcesResult(results),
                DatadogJsonContext.Default.DatadogMonitorResourcesResult) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the command.");
            HandleException(context.Response, ex);
        }

        return context.Response;
    }

    private static ArgumentBuilder<MonitoredResourcesListArguments> CreateDatadogResourceArgument() =>
        ArgumentBuilder<MonitoredResourcesListArguments>
            .Create(ArgumentDefinitions.Datadog.DatadogResource.Name, ArgumentDefinitions.Datadog.DatadogResource.Description)
            .WithValueAccessor(args => args.DatadogResource ?? string.Empty)
            .WithIsRequired(true);

    internal record DatadogMonitorResourcesResult(DatadogMonitorResourceModel resources);
}


