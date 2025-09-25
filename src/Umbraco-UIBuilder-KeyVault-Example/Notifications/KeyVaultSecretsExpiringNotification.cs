using Umbraco.Cms.Core.Notifications;
using Umbraco_UIBuilder_KeyVault_Example.Entities;

namespace Umbraco_UIBuilder_KeyVault_Example.Notifications;

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