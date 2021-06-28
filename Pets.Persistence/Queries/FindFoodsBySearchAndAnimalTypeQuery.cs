namespace Pets.Persistence.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Criteria;
    using Domain.Entities;
    using global::Database.Abstractions;
    using global::Queries.Abstractions;


    public class FindFoodsBySearchAndAnimalTypeQuery : IAsyncQuery<FindBySearchAndAnimalType, List<Food>>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public FindFoodsBySearchAndAnimalTypeQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<List<Food>> AskAsync(
            FindBySearchAndAnimalType criterion,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Food, bool>> expr = null;

            if (!string.IsNullOrWhiteSpace(criterion.Search))
            {
                expr = x => x.Name.Contains(criterion.Search);
            }

            if (criterion.AnimalType.HasValue)
            {
                Expression<Func<Food, bool>> secondExpr = x => x.AnimalType == criterion.AnimalType;
                expr = expr != null ? expr.AndAlso(secondExpr) : secondExpr;
            }

            return expr == null ? _dbContext.Foods.ToList() : _dbContext.Foods.Where(expr).ToList();
        }
    }
}