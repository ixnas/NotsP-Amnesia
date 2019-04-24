using System;

namespace Amnesia.Application.Helper
{
    public class Maybe<T>
    {
        private readonly T value;
        private readonly bool hasItem;

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        public Maybe()
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
        {
            hasItem = false;
        }

        static Maybe()
        {
            if (typeof(T) == typeof(Maybe<T>))
            {
                throw new ArgumentException("T cannot be a maybe!");
            }
        }

        public Maybe(T value)
        {
            this.value = value;
            hasItem = true;
        }

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return hasItem 
                ? new Maybe<TResult>(selector(value!)) 
                : new Maybe<TResult>();
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }
    }
}