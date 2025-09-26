using Umbraco.Community.SecretManager.Entities;

namespace Umbraco.Community.SecretManager.Services;

public interface IKeyVaultService
{
    List<SecretDetail> GetSecrets();
}