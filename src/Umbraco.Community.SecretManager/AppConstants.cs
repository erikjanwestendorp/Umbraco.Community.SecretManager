namespace Umbraco.Community.SecretManager;

public static class AppConstants
{
    public static TimeSpan DefaultWarnBefore = TimeSpan.FromDays(7);
    public static TimeSpan DefaultPeriod = TimeSpan.FromDays(1);
    

    public static class WebhookEvents
    {
        public const string SecretsExpiringAlias = "Custom.SecretsExpiring";
        public const string SecretsExpiringName = "Secrets Expiring";
    }
}
