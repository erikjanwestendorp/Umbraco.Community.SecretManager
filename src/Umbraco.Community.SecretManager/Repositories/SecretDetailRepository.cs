using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;
using Umbraco.Community.SecretManager.Entities;
using Umbraco.Community.SecretManager.Services;
using Umbraco.UIBuilder;
using Umbraco.UIBuilder.Persistence;

namespace Umbraco.Community.SecretManager.Repositories;

public class SecretDetailRepository(RepositoryContext context, IKeyVaultService keyVaultService) : Repository<SecretDetail, string>(context)
{
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
        return keyVaultService.GetSecrets();
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
