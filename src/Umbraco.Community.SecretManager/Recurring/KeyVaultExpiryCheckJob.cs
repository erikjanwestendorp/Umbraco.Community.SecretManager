using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.BackgroundJobs;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Entities;
using Umbraco.Community.SecretManager.Notifications;
using Umbraco.Community.SecretManager.Services;

namespace Umbraco.Community.SecretManager.Recurring;

internal class KeyVaultExpiryCheckJob(
    IKeyVaultService keyVault,
    IEventAggregator events,
    ILogger<KeyVaultExpiryCheckJob> logger,

    IOptions<SecretManagerOptions> opts) : IRecurringBackgroundJob
{
    private readonly SecretManagerOptions _opts = opts.Value;

    public TimeSpan Period => _opts.Period ?? TimeSpan.FromHours(1);

    public event EventHandler? PeriodChanged { add { } remove { } }

    public Task RunJobAsync()
    {
        var nowUtc = DateTime.UtcNow;
        var threshold = nowUtc.Add(_opts.WarnBefore ?? TimeSpan.FromDays(14));

        var expiring = new List<SecretDetail>();
        foreach (var secretDetail in keyVault.GetSecrets())
        {
            //if (string.Equals(secretDetail.Expire, "Never", StringComparison.OrdinalIgnoreCase))
            //{
            //    continue;
            //}


            //if (DateTime.TryParseExact(
            //        secretDetail.Expire,
            //        KeyVaultService.DateFormat,
            //        CultureInfo.InvariantCulture,
            //        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            //        out var expiresUtc)
            //    && expiresUtc <= threshold)
            //{
            //    expiring.Add(secretDetail);
            //}

            expiring.Add(secretDetail);
        }

        if (expiring.Count <= 0)
        {
            return Task.CompletedTask;
        }

        logger.LogInformation("Found {Count} Key Vault secrets nearing expiry.", expiring.Count);
        events.Publish(new KeyVaultSecretsExpiringNotification(nowUtc, threshold, expiring));

        return Task.CompletedTask;
    }
}
