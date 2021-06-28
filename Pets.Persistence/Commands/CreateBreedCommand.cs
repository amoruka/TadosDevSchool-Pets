namespace Pets.Persistence.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Commands.Contexts;
    using global::Commands.Abstractions;
    using global::Database.Abstractions;


    public class CreateBreedCommand : IAsyncCommand<CreateBreedCommandContext>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public CreateBreedCommand(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task ExecuteAsync(
            CreateBreedCommandContext commandContext,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Breeds.AddAsync(commandContext.Breed);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}