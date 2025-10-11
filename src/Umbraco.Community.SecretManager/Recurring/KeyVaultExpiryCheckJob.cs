using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Infrastructure.BackgroundJobs;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Entities;
using Umbraco.Community.SecretManager.Notifications;
using Umbraco.Community.SecretManager.Services;

namespace Umbraco.Community.SecretManager.Recurring;

internal class KeyVaultExpiryCheckJob(
    IKeyVaultService keyVaultService,
    IEventAggregator events,
    ILogger<KeyVaultExpiryCheckJob> logger,
    IOptions<SecretManagerOptions> opts) : IRecurringBackgroundJob
{
    private readonly SecretManagerOptions _opts = opts.Value;

    public TimeSpan Period => _opts.Period;
    public TimeSpan Delay { get; } = GetDelay(opts.Value);
    public event EventHandler? PeriodChanged { add { } remove { } }


    public Task RunJobAsync()
    {
        var nowUtc = DateTime.UtcNow;
        var threshold = nowUtc.Add(_opts.WarnBefore);

        var expiring = new List<SecretDetail>();
        foreach (var secretDetail in keyVaultService.GetSecrets())
        {
            if (secretDetail.ExpirationDate is null)
            {
                continue;
            }

            if (secretDetail.ExpirationDate > nowUtc &&
                secretDetail.ExpirationDate <= threshold)
            {
                expiring.Add(secretDetail);
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

    private static TimeSpan GetDelay(SecretManagerOptions secretManagerOptions)
    {
        var cron = CrontabSchedule.TryParse(secretManagerOptions.FirstRun);

        if(cron == null)
        {
            return TimeSpan.FromMinutes(3);
        }

        var now = DateTime.Now;
        var next = cron.GetNextOccurrence(now);

        return next - now;
    }
}
