namespace Umbraco.Community.SecretManager.Configuration;

public class SecretManagerOptions
{
    public bool EnableUmbracoUiBuilder { get; set; } = false;
    public TimeSpan Period { get; set; } = AppConstants.DefaultPeriod;
    public string? FirstRun { get; set; }
    public TimeSpan WarnBefore { get; set; } = AppConstants.DefaultWarnBefore;
    public string DateTimeFormat { get; set; } = AppConstants.DefaultDateTimeFormat;
    public string Culture { get; set; } = AppConstants.DefaultCulture;
}
