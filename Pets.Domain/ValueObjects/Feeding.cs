namespace Pets.Domain.ValueObjects
{
    using System;
    using global::Domain.Abstractions;
    using Entities;

    public class Feeding : IValueObjectWithId
    {
        [Obsolete("Only for reflection", true)]
        public Feeding()
        {
        }
        
        protected internal Feeding(DateTime dateTimeUtc, Food food, Animal animal, int count)
        {
            if (count < 0) 
                throw new ArgumentOutOfRangeException(nameof(count));

            DateTimeUtc = dateTimeUtc;
            Food = food ?? throw new ArgumentNullException(nameof(food));
            Animal = animal ?? throw new ArgumentNullException(nameof(animal));
            Count = count;
        }

        public Feeding(long id, DateTime dateTimeUtc, Food food, Animal animal, int count)
            : this(dateTimeUtc, food, animal, count)
        {
            Id = id;
        }



        public long Id { get; set; }

        public DateTime DateTimeUtc { get; init; }

        public Food Food { get; init; }

        public Animal Animal { get; init; }

        public int Count { get; init; }
    }
}
