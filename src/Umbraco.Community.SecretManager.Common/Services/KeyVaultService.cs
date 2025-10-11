using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;

namespace Umbraco.Community.SecretManager.Common.Services;

public class KeyVaultService(IMemoryCache memoryCache, SecretClient secretClient) : IKeyVaultService
{
    public List<SecretProperties> GetSecrets()
    {
        const string cacheKey = "secrets";

        if (memoryCache.TryGetValue(cacheKey, out List<SecretProperties>? cachedSecrets))
        {
            if(cachedSecrets != null)
            {
                return cachedSecrets;
            }
        }

        var result = secretClient.GetPropertiesOfSecrets().ToList();

        memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return result;
    }
}
