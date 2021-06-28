namespace Pets.Persistence.Queries
{
    using System;
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using Domain.Criteria;
    using global::Database.Abstractions;
    using global::Queries.Abstractions;
    using Microsoft.EntityFrameworkCore;

    public class FindAnimalsCountByNameAndAnimalTypeQuery : IAsyncQuery<FindAnimalsCountByNameAndAnimalType, int>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public FindAnimalsCountByNameAndAnimalTypeQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<int> AskAsync(
            FindAnimalsCountByNameAndAnimalType criterion,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Animals
                .CountAsync(x => x.Name == criterion.Name && x.Type == criterion.AnimalType, cancellationToken);
        }
    }
}