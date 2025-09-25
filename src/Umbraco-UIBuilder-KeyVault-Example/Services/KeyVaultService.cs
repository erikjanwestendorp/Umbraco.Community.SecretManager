using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Umbraco_UIBuilder_KeyVault_Example.Configuration;
using Umbraco_UIBuilder_KeyVault_Example.Entities;

namespace Umbraco_UIBuilder_KeyVault_Example.Services;

internal class KeyVaultService(SecretClient secretClient, IMemoryCache memoryCache, IOptions<KeyVaultOptions> options) : IKeyVaultService
{
    public const string DateFormat = "yyyy-MM-dd";
    public string CacheKey = options.Value.Endpoint;

    public List<SecretDetail> GetSecrets()
    {
        if(memoryCache.TryGetValue("secrets", out List<SecretDetail>? cachedSecrets))
        {
            return cachedSecrets;
        }
        var result = new List<SecretDetail>();
        foreach (var prop in secretClient.GetPropertiesOfSecrets())
        {
            var expiresOn = prop.ExpiresOn?.UtcDateTime;

            result.Add(new SecretDetail
            {
                Name = prop.Name,
                CreatedOn = prop.CreatedOn != null ? prop.CreatedOn!.ToString()! : string.Empty,
                Expire = expiresOn?.ToString(DateFormat) ?? "Never",
                RecoveryLevel = prop.RecoveryLevel ?? "N/A",
                Tags = prop.Tags != null ? string.Join(", ", prop.Tags.Select(t => $"{t.Key}:{t.Value}")) : "No Tags"
            });
        }

        return result;
    }
}
