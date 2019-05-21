using System;

namespace Amnesia.Application.Helper
{
    public class Maybe<T>
    {
        public T Value { get; }
        public bool HasValue { get; }

        public Maybe()
        {
            Value = default;
            HasValue = false;
        }

        public Maybe(T value)
        {
            Value = value;
            HasValue = true;
        }

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return HasValue 
                ? new Maybe<TResult>(selector(Value!)) 
                : new Maybe<TResult>();
        }

        public static implicit operator Maybe<T>(T value)
        {
            return value == null 
                ? new Maybe<T>() 
                : new Maybe<T>(value);
        }
    }

    public class Maybe
    {
        public static Maybe<T> Some<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> None<T>()
        {
            return new Maybe<T>();
        }
    }
}