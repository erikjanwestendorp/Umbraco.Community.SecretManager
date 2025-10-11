namespace Umbraco.Community.SecretManager.Configuration;

public class SecretManagerRecurringOptions
{
    public TimeSpan Period { get; set; } = AppConstants.DefaultPeriod;
    public string? FirstRun { get; set; }
    public TimeSpan WarnBefore { get; set; } = AppConstants.DefaultWarnBefore;
}
