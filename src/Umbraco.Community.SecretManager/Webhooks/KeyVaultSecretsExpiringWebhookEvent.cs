using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Core.Webhooks;
using Umbraco.Community.SecretManager.Notifications;

namespace Umbraco.Community.SecretManager.Webhooks;

[WebhookEvent(AppConstants.WebhookEvents.SecretsExpiringName, UmbConstants.WebhookEvents.Types.Other)]
public sealed class KeyVaultSecretsExpiringWebhookEvent(
    IWebhookFiringService firingService,
    IWebhookService webhookService,
    IOptionsMonitor<WebhookSettings> webhookSettings,
    IServerRoleAccessor serverRoleAccessor)
    : WebhookEventBase<KeyVaultSecretsExpiringNotification>(firingService, webhookService, webhookSettings,
        serverRoleAccessor)
{
    public override string Alias => AppConstants.WebhookEvents.SecretsExpiringAlias;

    public override object? ConvertNotificationToRequestPayload(KeyVaultSecretsExpiringNotification notification) => new
    {
        Results = notification.Secrets.Select(result => new
        {
            result.Name,
            result.Expire,
        })
    };
}