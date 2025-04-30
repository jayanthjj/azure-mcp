using System.CommandLine;
using System.CommandLine.Parsing;
using AzureMcp.Models.Command;
using AzureMcp.Services.Interfaces;
using Microsoft.Extensions.Logging;
using AzureMcp.Arguments.Datadog.MonitoredResources;
using AzureMcp.Models.Argument;
using ModelContextProtocol.Server;

namespace AzureMcp.Commands.Datadog.MonitoredResources;

public sealed class MonitorCreateCommand(ILogger<MonitorCreateCommand> logger) : SubscriptionCommand<MonitorCreateArguments>()
{

    protected readonly Option<string> _monitorResourceOption = ArgumentDefinitions.Datadog.MonitorName.ToOption();
    protected readonly Option<string> _locationOption = ArgumentDefinitions.Datadog.Location.ToOption();
    protected readonly Option<string> _apiKeyOptions = ArgumentDefinitions.Datadog.ApiKey.ToOption();
    protected readonly Option<string> _applicationKeyOptions = ArgumentDefinitions.Datadog.ApplicationKey.ToOption();

    protected override string GetCommandName() => "create";

    protected override string GetCommandDescription() =>
        $"""
        Creates a new Datadog monitor in the specified resource group and location.
        The command requires the monitor name, resource group, location, API key, and application key as inputs.
        The required parameters to be taken as input from the user are {ArgumentDefinitions.Datadog.MonitorName}, {ArgumentDefinitions.Datadog.Location}, {ArgumentDefinitions.Datadog.ApiKey}, and {ArgumentDefinitions.Datadog.ApplicationKey}.
        The command will create a new Datadog monitor with the specified parameters.
        """;

    protected override MonitorCreateArguments BindArguments(ParseResult parseResult)
    {
        var args = base.BindArguments(parseResult);
        args.MonitorName = parseResult.GetValueForOption(_monitorResourceOption);
        args.Location = parseResult.GetValueForOption(_locationOption);
        args.ApiKey = parseResult.GetValueForOption(_apiKeyOptions);
        args.ApplicationKey = parseResult.GetValueForOption(_applicationKeyOptions);
        args.ResourceGroup = parseResult.GetValueForOption(_resourceGroupOption) ?? ArgumentDefinitions.Common.ResourceGroup.DefaultValue;
        return args;
    }
    
    protected override void RegisterOptions(Command command)
    {
        base.RegisterOptions(command);
        command.AddOption(_monitorResourceOption);
        command.AddOption(_locationOption);
        command.AddOption(_apiKeyOptions);
        command.AddOption(_applicationKeyOptions);
        command.AddOption(_resourceGroupOption);
    }

    protected override void RegisterArguments()
    {
        base.RegisterArguments();
        AddArgument(CreateMonitorNameArgument());
        AddArgument(CreateLocationArgument());
        AddArgument(CreateApiKeyArgument());
        AddArgument(CreateApplicationKeyArgument());
        AddArgument(CreateResourceGroupArgument());
    }

    private static ArgumentBuilder<MonitorCreateArguments> CreateMonitorNameArgument() =>
        ArgumentBuilder<MonitorCreateArguments>
            .Create(ArgumentDefinitions.Datadog.MonitorName.Name, ArgumentDefinitions.Datadog.MonitorName.Description)
            .WithValueAccessor(args => args.MonitorName ?? string.Empty)
            .WithIsRequired(true);

    private static ArgumentBuilder<MonitorCreateArguments> CreateLocationArgument() =>
        ArgumentBuilder<MonitorCreateArguments>
            .Create(ArgumentDefinitions.Datadog.Location.Name, ArgumentDefinitions.Datadog.Location.Description)
            .WithValueAccessor(args => args.Location ?? string.Empty)
            .WithIsRequired(true);

    private static ArgumentBuilder<MonitorCreateArguments> CreateApiKeyArgument() =>
        ArgumentBuilder<MonitorCreateArguments>
            .Create(ArgumentDefinitions.Datadog.ApiKey.Name, ArgumentDefinitions.Datadog.ApiKey.Description)
            .WithValueAccessor(args => args.ApiKey ?? string.Empty)
            .WithIsRequired(true);

    private static ArgumentBuilder<MonitorCreateArguments> CreateApplicationKeyArgument() =>
        ArgumentBuilder<MonitorCreateArguments>
            .Create(ArgumentDefinitions.Datadog.ApplicationKey.Name, ArgumentDefinitions.Datadog.ApplicationKey.Description)
            .WithValueAccessor(args => args.ApplicationKey ?? string.Empty)
            .WithIsRequired(true);

    [McpServerTool(Destructive = false, ReadOnly = true)]

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
            var result = await service.CreateMonitor(
                args.Subscription!,
                args.MonitorName,
                args.ResourceGroup!,
                args.Location,
                args.ApiKey,
                args.ApplicationKey);

            context.Response.Results = result.Count>0 ? new { result} : null;

            //context.Response.Results = new { Message = "Monitor created successfully." };
        }
        catch (Exception ex)
        {
            HandleException(context.Response, ex);
        }

        return context.Response;
    }
}