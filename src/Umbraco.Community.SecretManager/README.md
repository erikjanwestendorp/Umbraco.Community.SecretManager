# SecretManager

A lightweight and flexible Azure Key Vault Secret Manager for Umbraco CMS. Receive notifications when secrets are about to expire, send alerts via webhooks, and display an overview of all secrets directly in the Umbraco backoffice.

![Secrets Overview](https://raw.githubusercontent.com/erikjanwestendorp/Umbraco.Community.SecretManager/main/assets/secrets-overview.png)

## Getting Started

### Prerequisites
- An Azure Key Vault with secrets configured
- Azure credentials (e.g., Azure CLI for development, DefaultAzureCredential for production)
- Umbraco CMS 16.x

### Step 1: Configure Azure Key Vault
Set up your Azure Key Vault endpoint in `appsettings.json`:

```json
{
  "KeyVault": {
    "Endpoint": "https://your-keyvault-name.vault.azure.net/"
  }
}
```

### Step 2: Create the SecretClient
In your `Program.cs`, configure the `SecretClient` to connect to Azure Key Vault:

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var keyVaultEndpoint = builder.Configuration["KeyVault:Endpoint"];
var credential = builder.Environment.IsDevelopment()
    ? new AzureCliCredential()
    : new DefaultAzureCredential();

var secretClient = new SecretClient(new Uri(keyVaultEndpoint), credential);
```

### Step 3: Register SecretManager
Add the SecretManager services to your Umbraco builder:

```csharp
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .ConfigureSecretManager(secretClient)
    .Build();
```

## Adding the UI Dashboard

To display an overview of all secrets in the Umbraco backoffice, install the UI package and register it:

```bash
dotnet add package Umbraco.Community.SecretManager.UIBuilder
```

Then add the following to your configuration:

```csharp
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .ConfigureSecretManager(secretClient)
    .ConfigureSecretManagerUI()
    .Build();
```

The dashboard shows secret details including Name, Expiration Date, Created On, Recovery Level, and Tags.

## Configuration Options

Configure the recurring expiry check job in `appsettings.json`:

```json
{
  "SecretManager": {
    "Recurring": {
      "Period": "1.00:00:00",
      "FirstRun": "0 8 * * *",
      "WarnBefore": "7.00:00:00"
    },
    "UIBuilder": {
      "DateTimeFormat": "MMMM d, yyyy",
      "Culture": "en-US"
    }
  }
}
```

| Option | Description | Default |
|--------|-------------|---------|
| `Period` | How often to check for expiring secrets | 1 day |
| `FirstRun` | Cron expression for scheduling the first job run (e.g., `0 8 * * *` runs daily at 8 AM) | Default delay |
| `WarnBefore` | Time before expiration to trigger alerts | 7 days |

## Setting Up Webhooks

Configure webhooks in the Umbraco backoffice to receive notifications when secrets are about to expire.

![Webhooks Configuration](https://raw.githubusercontent.com/erikjanwestendorp/Umbraco.Community.SecretManager/main/assets/webhooks.png)

1. Navigate to **Settings** → **Webhooks**.
2. Create a new webhook.
3. Select **"Secrets Expiring"** as the event.
4. Enter your webhook endpoint URL.
5. Save the webhook.

## Custom Webhook Payload Providers

Create custom payload formats for different webhook endpoints (e.g., Microsoft Teams via Power Automate):

```csharp
using Umbraco.Cms.Core.Models;
using Umbraco.Community.SecretManager.Notifications;
using Umbraco.Community.SecretManager.Webhooks;

public class TeamsSecretsExpiringProvider : WebhookPayloadProviderBase<KeyVaultSecretsExpiringNotification>
{
    protected override bool CanHandle(Uri endpoint, string eventAlias, 
        KeyVaultSecretsExpiringNotification notification, IWebhook webhook)
    {
        // Matches Power Automate webhook endpoints for Teams integration
        return endpoint.Host.EndsWith("environment.api.powerplatform.com");
    }
    
    protected override object BuildPayload(KeyVaultSecretsExpiringNotification notification, 
        Uri endpoint, string eventAlias, IWebhook webhook)
    {
        return new
        {
            type = "message",
            attachments = new[] {
                new {
                    contentType = "application/vnd.microsoft.card.adaptive",
                    content = new {
                        type = "AdaptiveCard",
                        version = "1.4",
                        body = notification.Secrets.Select(s => new { 
                            type = "TextBlock", 
                            text = $"{s.Name} expires: {s.ExpiresOn?.UtcDateTime:yyyy-MM-dd}" 
                        }).ToArray()
                    }
                }
            }
        };
    }
}
```

Register the custom provider:

```csharp
builder.WithCollectionBuilder<WebhookPayloadProviderCollectionBuilder>()
    .Add<TeamsSecretsExpiringProvider>();
```