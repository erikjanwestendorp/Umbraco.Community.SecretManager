using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;
using Umbraco.Community.SecretManager.Common.Services;
using Umbraco.Community.SecretManager.UIBuilder.Entities;
using Umbraco.UIBuilder.Persistence;
using Umbraco.UIBuilder;
using System.Globalization;
using Microsoft.Extensions.Options;
using Umbraco.Community.SecretManager.UIBuilder.Configuration;

namespace Umbraco.Community.SecretManager.UIBuilder.Repositories;

public class SecretDetailRepository(RepositoryContext context, IKeyVaultService keyVaultService, IOptions<SecretManagerUIBuilderOptions> opts) : Repository<SecretDetail, string>(context)
{
    private readonly SecretManagerUIBuilderOptions _options = opts.Value;

    protected override string GetIdImpl(SecretDetail entity)
    {
        return entity.Name;
    }

    protected override SecretDetail GetImpl(string name)
    {
        return GetAllImpl().First(s => s.Name == name);
    }

    protected override IEnumerable<TJunctionEntity> GetRelationsByParentIdImpl<TJunctionEntity>(string parentId, string relationAlias)
    {
        return [];
    }

    protected override SecretDetail SaveImpl(SecretDetail entity)
    {
        return entity;
    }

    protected override TJunctionEntity SaveRelationImpl<TJunctionEntity>(TJunctionEntity entity)
    {
        return entity;
    }

    protected override void DeleteImpl(string name)
    {
        return;
    }

    protected override IEnumerable<SecretDetail> GetAllImpl(Expression<Func<SecretDetail, bool>>? whereClause = null, Expression<Func<SecretDetail, object>>? orderBy = null,
        SortDirection orderByDirection = SortDirection.Ascending)
    {
        var secrets = keyVaultService.GetSecrets();

        var result = new List<SecretDetail>();
        foreach (var secret in secrets)
        {
            var expiresOn = secret.ExpiresOn?.UtcDateTime;

            result.Add(new SecretDetail
            {
                Name = secret.Name,
                CreatedOn = secret.CreatedOn != null ? secret.CreatedOn!.ToString()! : string.Empty,
                ExpirationPreview = expiresOn?.ToString(_options.DateTimeFormat, new CultureInfo(_options.Culture)) ?? "N/A",
                ExpirationDate = expiresOn,
                RecoveryLevel = secret.RecoveryLevel ?? "N/A",
                Tags = secret.Tags != null ? string.Join(", ", secret.Tags.Select(t => $"{t.Key}:{t.Value}")) : "No Tags"
            });
        }

        return result;
    }

    protected override PagedResult<SecretDetail> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<SecretDetail, bool>>? whereClause = null, Expression<Func<SecretDetail, object>>? orderBy = null,
        SortDirection orderByDirection = SortDirection.Ascending)
    {
        var query = GetAllImpl().AsQueryable();

        if (whereClause != null)
        {
            query = query.Where(whereClause);
        }


        if (orderBy != null)
        {
            query = orderByDirection == SortDirection.Ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
        }

        var totalCount = query.Count();

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<SecretDetail>(totalCount, pageNumber, pageSize)
        {
            Items = items
        };
    }

    protected override long GetCountImpl(Expression<Func<SecretDetail, bool>> whereClause)
    {
        return GetAllImpl().Count();
    }
}