using AzureMcp.Arguments.Datadog.MonitoredResources;
using AzureMcp.Commands;
using AzureMcp.Models.Argument;
using AzureMcp.Models.Command;
using AzureMcp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace AzureMcp.Commands.Datadog.MonitoredResources;

public sealed class MonitoredResourcesListCommand(ILogger<MonitoredResourcesListCommand> logger) : SubscriptionCommand<MonitoredResourcesListArguments>()
{

    protected override string GetCommandName() => "list";

    protected override string GetCommandDescription() =>
    $"""
    Lists monitored resources in Datadog for a datadog resource taken as input from the user in {ArgumentDefinitions.Datadog.DatadogResource}. 
    The command will display the top 20 monitored resources by default and provide an option to view the rest if requested.
    """;

    protected readonly Option<string> _datadogResourceOption = ArgumentDefinitions.Datadog.DatadogResource.ToOption();

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_datadogResourceOption);
        command.AddOption(_resourceGroupOption);
    }

    protected override MonitoredResourcesListArguments BindArguments(ParseResult parseResult)
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
            var results = await service.ListMonitoredResources(
                args.ResourceGroup!,
                args.Subscription!,
                args.Tenant!,
                args.DatadogResource);

            context.Response.Results = results?.Count > 0 ? results : null;
        }
        catch (Exception ex)
        {
            HandleException(context.Response, ex);
        }

        return context.Response;
    }

    protected override void RegisterArguments()
    {
        base.RegisterArguments();
        AddArgument(CreateDatabaseResourceArgument());
        AddArgument(CreateResourceGroupArgument());
    }

    private static ArgumentBuilder<MonitoredResourcesListArguments> CreateDatabaseResourceArgument() =>
        ArgumentBuilder<MonitoredResourcesListArguments>
            .Create(ArgumentDefinitions.Datadog.DatadogResource.Name, ArgumentDefinitions.Datadog.DatadogResource.Description)
            .WithValueAccessor(args => args.DatadogResource ?? string.Empty)
            .WithIsRequired(true);
}