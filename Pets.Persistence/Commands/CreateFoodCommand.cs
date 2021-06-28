namespace Pets.Persistence.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Commands.Contexts;
    using global::Commands.Abstractions;
    using global::Database.Abstractions;

    public class CreateFoodCommand : IAsyncCommand<CreateFoodCommandContext>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public CreateFoodCommand(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task ExecuteAsync(
            CreateFoodCommandContext commandContext,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Foods.AddAsync(commandContext.Food);

            await _dbContext.SaveChangesAsync();
        }
    }
}