namespace Pets.Persistence.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Domain.Criteria;
    using Domain.Entities;
    using global::Database.Abstractions;
    using global::Queries.Abstractions;


    public class FindBreedsBySearchAndAnimalTypeQuery : IAsyncQuery<FindBySearchAndAnimalType, List<Breed>>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public FindBreedsBySearchAndAnimalTypeQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<List<Breed>> AskAsync(
            FindBySearchAndAnimalType criterion,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Breed, bool>> expr = null;

            if (!string.IsNullOrWhiteSpace(criterion.Search))
            {
                expr = x => x.Name.Contains(criterion.Search);
            }

            if (criterion.AnimalType.HasValue)
            {
                Expression<Func<Breed, bool>> secondExpr = x => x.AnimalType == criterion.AnimalType;
                expr = expr != null ? expr.AndAlso(secondExpr) : secondExpr;
            }

            return expr == null ? _dbContext.Breeds.ToList() : _dbContext.Breeds.Where(expr).ToList();
        }
    }
}