using Umbraco_UIBuilder_KeyVault_Example.Entities;

namespace Umbraco_UIBuilder_KeyVault_Example.Services;

public interface IKeyVaultService
{
    List<SecretDetail> GetSecrets();
}