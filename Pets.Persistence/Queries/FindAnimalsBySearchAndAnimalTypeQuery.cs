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
    using Microsoft.EntityFrameworkCore;
    using Pets.Persistence;


    public class FindAnimalsBySearchAndAnimalTypeQuery : IAsyncQuery<FindBySearchAndAnimalType, List<Animal>>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public FindAnimalsBySearchAndAnimalTypeQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<List<Animal>> AskAsync(
            FindBySearchAndAnimalType criterion,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Animal, bool>> expr = null;

            if (!string.IsNullOrWhiteSpace(criterion.Search))
            {
                expr = x => x.Name.Contains(criterion.Search);
            }

            if (criterion.AnimalType.HasValue)
            {
                Expression<Func<Animal, bool>> secondExpr = x => x.Type == criterion.AnimalType;
                expr = expr != null ? expr.AndAlso(secondExpr) : secondExpr;
            }

            IQueryable<Animal> animalsSet;

            if (expr == null)
            {
                animalsSet = _dbContext.Animals;
            }
            else
            {
                animalsSet = _dbContext.Animals.Where(expr);
            }

            // Eager loading
            var animals = animalsSet
                .Include(animal => animal.Feedings)
                .ThenInclude(feeding => feeding.Food)
                .Include(animal => animal.Breed)
                .ToList();

            return animals;
        }
    }
}