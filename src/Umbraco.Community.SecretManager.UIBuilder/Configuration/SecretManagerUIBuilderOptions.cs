namespace Umbraco.Community.SecretManager.UIBuilder.Configuration;

// ReSharper disable once InconsistentNaming
public class SecretManagerUIBuilderOptions
{
    public string DateTimeFormat { get; set; } = UIConstants.DefaultDateTimeFormat;
    public string Culture { get; set; } = UIConstants.DefaultCulture;
}