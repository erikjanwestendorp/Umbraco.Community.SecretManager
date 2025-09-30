using Umbraco.Cms.Core.Models;
using Umbraco.Community.SecretManager.Notifications;
using Umbraco.Community.SecretManager.Webhooks;

namespace Umbraco.Community.SecretManager.Site.WebhookPayloadProviders;

internal class TeamsSecretsExpiringProvider : WebhookPayloadProviderBase<KeyVaultSecretsExpiringNotification>
{
    protected override bool CanHandle(Uri endpoint, string eventAlias, KeyVaultSecretsExpiringNotification notification, IWebhook webhook)
    {
        var host = endpoint.Host.ToLowerInvariant();
        var isTeams = host.EndsWith("environment.api.powerplatform.com");
        return isTeams && string.Equals(eventAlias, AppConstants.WebhookEvents.SecretsExpiringAlias, StringComparison.OrdinalIgnoreCase);
    }
    
    protected override object BuildPayload(KeyVaultSecretsExpiringNotification notification, Uri _, string __, IWebhook ___)
        => new
        {
            type = "message",
            attachments = new[] {
                new {
                    contentType = "application/vnd.microsoft.card.adaptive",
                    content = new {
                        type = "AdaptiveCard",
                        version = "1.4",
                        body = notification.Secrets.SelectMany(s => new object[] {
                            new { type="TextBlock", size="Medium", weight="Bolder", text=s.Name },
                            new { type="TextBlock", wrap=true, text=$"Expires: {s.ExpirationDate:yyyy-MM-dd HH:mm}" }
                        }).ToArray()
                    }
                }
            }
        };
}