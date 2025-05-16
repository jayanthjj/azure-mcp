# Azure MCP CLI Command Reference

## Global Args

The following args are available for all commands:

| Arg | Required | Default | Description |
|-----------|----------|---------|-------------|
| `--subscription` | Yes | - | Azure subscription ID for target resources |
| `--tenant-id` | No | - | Azure tenant ID for authentication |
| `--auth-method` | No | 'credential' | Authentication method ('credential', 'key', 'connectionString') |
| `--retry-max-retries` | No | 3 | Maximum retry attempts for failed operations |
| `--retry-delay` | No | 2 | Delay between retry attempts (seconds) |
| `--retry-max-delay` | No | 10 | Maximum delay between retries (seconds) |
| `--retry-mode` | No | 'exponential' | Retry strategy ('fixed' or 'exponential') |
| `--retry-network-timeout` | No | 100 | Network operation timeout (seconds) |

## Available Commands

### Server Operations
```bash
# Start the MCP Server
azmcp server start [--transport <transport>]
```

### Subscription Management
```bash
# List available Azure subscriptions
azmcp subscription list [--tenant-id <tenant-id>]
```

### Best Practices
```bash
# Get secure, production-grade Azure SDK best practices for effective code generation.
azmcp bestpractices get
```

### Azure Cosmos DB Operations
```bash
# List Cosmos DB accounts in a subscription
azmcp cosmos account list --subscription <subscription>

# List databases in a Cosmos DB account
azmcp cosmos database list --subscription <subscription> --account-name <account-name>

# List containers in a Cosmos DB database
azmcp cosmos database container list --subscription <subscription> --account-name <account-name> --database-name <database-name>

# Query items in a Cosmos DB container
azmcp cosmos database container item query --subscription <subscription> \
                       --account-name <account-name> \
                       --database-name <database-name> \
                       --container-name <container-name> \
                       [--query "SELECT * FROM c"]
```

### Kusto (Azure Data Explorer) Operations
```bash
# List Kusto clusters in a subscription
azmcp kusto cluster list --subscription <subscription>

# Get details for a Kusto cluster
azmcp kusto cluster get --subscription <subscription> --cluster-name <cluster-name>

# List databases in a Kusto cluster
azmcp kusto database list [--cluster-uri <cluster-uri> | --subscription <subscription> --cluster-name <cluster-name>]

# List tables in a Kusto database
azmcp kusto table list [--cluster-uri <cluster-uri> | --subscription <subscription> --cluster-name <cluster-name>]
                                --database-name <database-name> \

# Retrieves the schema of a specified Kusto table.
azmcp kusto table schema [--cluster-uri <cluster-uri> | --subscription <subscription> --cluster-name <cluster-name>]
                                  --database-name <database-name> \
                                  --table <table-name>

# Query a Kusto database
azmcp kusto query [--cluster-uri <cluster-uri> | --subscription <subscription> --cluster-name <cluster-name>]
                           --database-name <database-name> \
                           --query "<kql-query>"

# Retrieves a sample of data from a specified Kusto table.
azmcp kusto sample [--cluster-uri <cluster-uri> | --subscription <subscription> --cluster-name <cluster-name>]
                            --database-name <database-name> \
                            --table <table-name> \
                           [--limit <limit>]

```
                      
### Azure DB for PostgreSQL Operations

```bash
## Database commands

# List all databases in a PostgreSQL server
azmcp postgres database list --subscription <subscription> --resource-group <resource-group> --user-name <user> --server <server>

# Execute a query on a PostgreSQL database
azmcp postgres database query --subscription <subscription> --resource-group <resource-group> --user-name <user> --server <server> --database <database> --query <query>

## Table Commands

# List all tables in a PostgreSQL database
azmcp postgres table list --subscription <subscription> --resource-group <resource-group> --user-name <user> --server <server> --database <database>

# Get the schema of a specific table in a PostgreSQL database
azmcp postgres table schema --subscription <subscription> --resource-group <resource-group> --user-name <user> --server <server> --database <database> --table <table>

## Server Commands

# List all PostgreSQL servers in a subscription & resource group
azmcp postgres server list --subscription <subscription> --resource-group <resource-group> --user-name <user>

# Retrieve the configuration of a PostgreSQL server
azmcp postgres server config --subscription <subscription> --resource-group <resource-group> ----user-name <user> --server <server>

# Retrieve a specific parameter of a PostgreSQL server
azmcp postgres server param --subscription <subscription> --resource-group <resource-group> --user-name <user> --server <server> --param <parameter>
```

### Azure Storage Operations
```bash
# List Storage accounts in a subscription
azmcp storage account list --subscription <subscription>

# List tables in a Storage account
azmcp storage table list --subscription <subscription> --account-name <account-name>

# List blobs in a Storage container
azmcp storage blob list --subscription <subscription> --account-name <account-name> --container-name <container-name>

# List containers in a Storage blob service
azmcp storage blob container list --subscription <subscription> --account-name <account-name>

# Get detailed properties of a storage container
azmcp storage blob container details --subscription <subscription> --account-name <account-name> --container-name <container-name>
```

### Azure Monitor (Log Analytics) Operations
```bash
# List Log Analytics workspaces in a subscription
azmcp monitor workspace list --subscription <subscription>

# List tables in a Log Analytics workspace
azmcp monitor table list --subscription <subscription> --workspace <workspace> --resource-group <resource-group>

# Query logs from Azure Monitor using KQL
azmcp monitor log query --subscription <subscription> \
                        --workspace <workspace> \
                        --table-name <table-name> \
                        --query "<kql-query>" \
                        [--hours <hours>] \
                        [--limit <limit>]

# Examples:
# Query logs from a specific table
azmcp monitor log query --subscription <subscription> \
                        --workspace <workspace> \
                        --table-name "AppEvents_CL" \
                        --query "| order by TimeGenerated desc"
```

### Azure App Configuration Operations
```bash
# List App Configuration stores in a subscription
azmcp appconfig account list --subscription <subscription>

# List all key-value settings in an App Configuration store
azmcp appconfig kv list --subscription <subscription> --account-name <account-name> [--key <key>] [--label <label>]

# Show a specific key-value setting
azmcp appconfig kv show --subscription <subscription> --account-name <account-name> --key <key> [--label <label>]

# Set a key-value setting
azmcp appconfig kv set --subscription <subscription> --account-name <account-name> --key <key> --value <value> [--label <label>]

# Lock a key-value setting (make it read-only)
azmcp appconfig kv lock --subscription <subscription> --account-name <account-name> --key <key> [--label <label>]

# Unlock a key-value setting (make it editable)
azmcp appconfig kv unlock --subscription <subscription> --account-name <account-name> --key <key> [--label <label>]

# Delete a key-value setting
azmcp appconfig kv delete --subscription <subscription> --account-name <account-name> --key <key> [--label <label>]
```

### Azure Key Vault Operations
```bash
# Lists keys in vault
azmcp keyvault key list --subscription <subscription> --vault <vault-name> --include-managed <true/false>

# Gets a key in vault
azmcp keyvault key get --subscription <subscription> --vault <vault-name> --key <key-name>

# Create a key in vault
azmcp keyvault key create --subscription <subscription> --vault <vault-name> --key <key-name> --key-type <key-type>
```

### Azure Service Bus Operations
```bash
# Peeks at messages in a Service Bus queue
azmcp servicebus queue peek --subscription <subscription> --namespace <service-bus-namespace> --queue-name <queue-name> [--max-messages <int>]

# Returns runtime and details about the Service Bus queue
azmcp servicebus queue details --subscription <subscription> --namespace <service-bus-namespace> --queue-name <queue-name>

# Gets runtime details a Service Bus topic
azmcp servicebus topic details --subscription <subscription> --namespace <service-bus-namespace> --topic-name <topic-name>

# Peeks at messages in a Service Bus subscription within a topic.
azmcp servicebus topic subscription peek --subscription <subscription> --namespace <service-bus-namespace> --topic-name <topic-name> --subscription-name <subscription-name> [--max-messages <int>]

# Gets runtime details and message counts for a Service Bus subscription
azmcp servicebus topic subscription details --subscription <subscription> --namespace <service-bus-namespace> --topic-name <topic-name> --subscription-name <subscription-name>
```

### Azure Native ISV Operations
```bash
# List monitored resources in Datadog
azmcp datadog monitored-resources list --subscription <subscription> --resource-group <resource-group> --datadog-resource <datadog-resource>
```

### Azure Resource Group Operations
```bash
# List resource groups in a subscription
azmcp group list --subscription <subscription>
```

### Azure CLI Extension Operations
```bash
# Execute any Azure CLI command
azmcp extension az --command "<command>"

# Examples:
# List resource groups
azmcp extension az --command "group list"

# Get storage account details
azmcp extension az --command "storage account show --name <account-name> --resource-group <resource-group>"

# List virtual machines
azmcp extension az --command "vm list --resource-group <resource-group>"
```

## Response Format

All responses follow a consistent JSON format:
```json
{
  "status": "200|403|500, etc",
  "message": "",
  "args": [],
  "results": [],
  "duration": 123
}
```

## Error Handling

The CLI returns structured JSON responses for errors, including:
- Missing required args
- Invalid arg values
- Service availability issues
- Authentication errors