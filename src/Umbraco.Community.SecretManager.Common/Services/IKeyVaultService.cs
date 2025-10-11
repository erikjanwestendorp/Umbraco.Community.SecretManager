using Azure.Security.KeyVault.Secrets;

namespace Umbraco.Community.SecretManager.Common.Services;

public interface IKeyVaultService
{
    List<SecretProperties> GetSecrets();
}
