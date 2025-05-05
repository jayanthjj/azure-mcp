using System.CommandLine;
using System.CommandLine.Parsing;
using AzureMcp.Models.Command;
using AzureMcp.Services.Interfaces;
using AzureMcp.Commands;
using Microsoft.Extensions.Logging;
using AzureMcp.Arguments.Datadog.MonitoredResources;
using AzureMcp.Models.Argument;

namespace AzureMcp.Commands.Datadog.MonitoredResources;

public sealed class MonitoredResourcesListCommand : SubscriptionCommand<MonitoredResourcesListArguments>
{
    private readonly ILogger<MonitoredResourcesListCommand> _logger;
    private readonly Option<string> _datadogResourceOption;

    public MonitoredResourcesListCommand(ILogger<MonitoredResourcesListCommand> logger) : base()
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _datadogResourceOption = ArgumentDefinitions.Datadog.DatadogResource.ToOption();
    }

    protected override string GetCommandName() => "list";

    protected override string GetCommandDescription() =>
        $"Lists monitored resources in Datadog for a datadog resource taken as input from the user in {ArgumentDefinitions.Datadog.DatadogResource}. The command will display the top 20 monitored resources by default and provide an option to view the rest if requested.";

    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_datadogResourceOption);
    }

    protected override MonitoredResourcesListArguments BindArguments(ParseResult parseResult)
    {
        var args = base.BindArguments(parseResult);
        args.DatadogResource = parseResult.GetValueForOption(_datadogResourceOption);
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
                args.DatadogResource!);

            context.Response.Results = results?.Count > 0 ? results : null;
        }
        catch (Exception ex)
        {
            HandleException(context.Response, ex);
        }

        return context.Response;
    }
}