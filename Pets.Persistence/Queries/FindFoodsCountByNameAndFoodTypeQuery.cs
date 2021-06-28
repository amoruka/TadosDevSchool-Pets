namespace Pets.Persistence.Queries
{
    using System;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Domain.Criteria;
    using global::Database.Abstractions;
    using global::Queries.Abstractions;
    using Microsoft.EntityFrameworkCore;

    public class FindFoodsCountByNameAndFoodTypeQuery : IAsyncQuery<FindFoodsCountByNameAndAnimalType, int>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public FindFoodsCountByNameAndFoodTypeQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<int> AskAsync(
            FindFoodsCountByNameAndAnimalType criterion,
            CancellationToken cancellationToken = default)
        {
            DbTransaction transaction = await _dbTransactionProvider.GetCurrentTransactionAsync(cancellationToken);
            DbConnection connection = transaction.Connection;

            return await _dbContext.Foods.CountAsync(x => x.Name == criterion.Name && x.AnimalType == criterion.AnimalType);
        }
    }
}