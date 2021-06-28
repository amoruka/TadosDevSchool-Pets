namespace Pets.Persistence.Queries
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Criteria;
    using Domain.Entities;
    using global::Database.Abstractions;
    using global::Queries.Abstractions;
    using Microsoft.EntityFrameworkCore;

    public class FindAnimalByIdQuery : IAsyncQuery<FindById, Animal>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;

        public FindAnimalByIdQuery(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<Animal> AskAsync(FindById criterion, CancellationToken cancellationToken = default)
        {
            var animal = await _dbContext.Animals.SingleAsync(x => x.Id == criterion.Id);

            // Explicit loading
            _dbContext
                .Entry(animal)
                .Reference(x => x.Breed)
                .Load();

            _dbContext
                .Entry(animal)
                .Collection(x => x.Feedings)
                .Load();

            animal.Feedings.ToList().ForEach(x => _dbContext.Entry(x).Reference(f => f.Food).Load());
            
            return animal;
        }
    }
}