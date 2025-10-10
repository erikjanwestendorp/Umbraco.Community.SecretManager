namespace Umbraco.Community.SecretManager.Configuration;

public class SecretManagerOptions
{
    public bool EnableUmbracoUiBuilder { get; set; } = false;
    public TimeSpan Period { get; set; } = TimeSpan.FromDays(1);
    public string? FirstRun { get; set; }
    public TimeSpan? WarnBefore { get; set; }
    public string DateTimeFormat { get; set; } = "MMMM d, yyyy";
    public string Culture { get; set; } = "en-US";
}
