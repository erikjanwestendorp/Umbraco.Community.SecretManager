using Azure.Security.KeyVault.Secrets;
using Umbraco.Cms.Core.Notifications;

namespace Umbraco.Community.SecretManager.Notifications;

public sealed class KeyVaultSecretsExpiringNotification(
    DateTime checkedAtUtc,
    IReadOnlyCollection<SecretProperties> secrets)
    : INotification
{
    public DateTime CheckedAtUtc { get; } = checkedAtUtc;
    public IReadOnlyCollection<SecretProperties> Secrets { get; } = secrets;
}