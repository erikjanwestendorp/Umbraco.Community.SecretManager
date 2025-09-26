namespace Umbraco.Community.SecretManager.Configuration;

public class KeyVaultOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public TimeSpan? Period { get; set; }
    public TimeSpan? WarnBefore { get; set; }
}
