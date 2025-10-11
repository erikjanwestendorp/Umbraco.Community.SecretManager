using System.Globalization;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Entities;

namespace Umbraco.Community.SecretManager.Services;

internal class KeyVaultService(SecretClient secretClient, IMemoryCache memoryCache, IOptions<SecretManagerOptions> options) : IKeyVaultService
{
    private readonly SecretManagerOptions _opts = options.Value;

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
                ExpirationPreview = expiresOn?.ToString(_opts.DateTimeFormat, new CultureInfo(_opts.Culture)) ?? "N/A",
                ExpirationDate = expiresOn,
                RecoveryLevel = prop.RecoveryLevel ?? "N/A",
                Tags = prop.Tags != null ? string.Join(", ", prop.Tags.Select(t => $"{t.Key}:{t.Value}")) : "No Tags"
            });
        }
        
        return result;
    }
}
