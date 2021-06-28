namespace Pets.Persistence.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Commands.Contexts;
    using global::Commands.Abstractions;
    using global::Database.Abstractions;


    public class CreateCatCommand : IAsyncCommand<CreateCatCommandContext>
    {
        private readonly IDbTransactionProvider _dbTransactionProvider;

        private readonly PetsContext _dbContext;


        public CreateCatCommand(IDbTransactionProvider dbTransactionProvider, PetsContext dbContext)
        {
            _dbTransactionProvider =
                dbTransactionProvider ?? throw new ArgumentNullException(nameof(dbTransactionProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task ExecuteAsync(
            CreateCatCommandContext commandContext,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Animals.AddAsync(commandContext.Cat);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}