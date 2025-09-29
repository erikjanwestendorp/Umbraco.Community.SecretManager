using Azure.Security.KeyVault.Secrets;

namespace Umbraco.Community.SecretManager.Configuration;

public class SecretManagerOptions
{
    public required SecretClient SecretClient { get; set; }
    public bool EnableUmbracoUiBuilder { get; set; } = false;
    public TimeSpan? Period { get; set; }
    public TimeSpan? WarnBefore { get; set; }
}
