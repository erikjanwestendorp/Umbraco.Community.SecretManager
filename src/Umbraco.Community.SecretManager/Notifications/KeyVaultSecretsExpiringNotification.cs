using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.SecretManager.Entities;

namespace Umbraco.Community.SecretManager.Notifications;

public sealed class KeyVaultSecretsExpiringNotification(
    DateTime checkedAtUtc,
    DateTime thresholdUtc,
    IReadOnlyCollection<SecretDetail> secrets)
    : INotification
{
    public DateTime CheckedAtUtc { get; } = checkedAtUtc;
    public DateTime ThresholdUtc { get; } = thresholdUtc;
    public IReadOnlyCollection<SecretDetail> Secrets { get; } = secrets;
}