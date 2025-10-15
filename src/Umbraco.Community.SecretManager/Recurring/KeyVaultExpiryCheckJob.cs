using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.BackgroundJobs;
using Umbraco.Community.SecretManager.Common.Services;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Notifications;

namespace Umbraco.Community.SecretManager.Recurring;

internal class KeyVaultExpiryCheckJob(
    IKeyVaultService keyVaultService,
    IEventAggregator events,
    ILogger<KeyVaultExpiryCheckJob> logger,
    IOptions<SecretManagerRecurringOptions> opts) : IRecurringBackgroundJob
{
    private readonly SecretManagerRecurringOptions _opts = opts.Value;

    public TimeSpan Period => _opts.Period;
    public TimeSpan Delay { get; } = GetDelay(opts.Value, logger);
    public event EventHandler? PeriodChanged { add { } remove { } }


    public Task RunJobAsync()
    {
        var nowUtc = DateTime.UtcNow;
        var threshold = nowUtc.Add(_opts.WarnBefore);

        logger.LogInformation("Checking Key Vault secrets for expiry before {Threshold} (UTC)", threshold);

        var expiring = new List<SecretProperties>();
        foreach (var secretProperties in keyVaultService.GetSecrets())
        {
            var expiresOn = secretProperties.ExpiresOn?.UtcDateTime;

            if (expiresOn > nowUtc &&
                expiresOn <= threshold)
            {
                expiring.Add(secretProperties);
            }

        }

        logger.LogInformation("Found {Count} Key Vault secrets nearing expiry.", expiring.Count);

        if (expiring.Count <= 0)
        {
            return Task.CompletedTask;
        }

        events.Publish(new KeyVaultSecretsExpiringNotification(nowUtc, expiring));

        return Task.CompletedTask;
    }

    private static TimeSpan GetDelay(SecretManagerRecurringOptions secretManagerOptions, ILogger<KeyVaultExpiryCheckJob> logger)
    {
        var cron = CrontabSchedule.TryParse(secretManagerOptions.FirstRun);

        if(cron == null)
        {
            logger.LogInformation("No valid cron expression found for first run, defaulting to 3 minutes.");
            return TimeSpan.FromMinutes(3);
        }

        var now = DateTime.Now;
        var next = cron.GetNextOccurrence(now);
        var delay = next - now;

        logger.LogInformation("First run scheduled at {FirstRun} (in {Delay}).", next, delay);
        return next - now;
    }
}
