using System;

namespace Functional.Either
{
    /// <summary>
    /// A functional monadic concept Either to make validation code more expressive and easier to maintain.
    /// <typeparam name="TResult">Type of Result item</typeparam>
    /// <typeparam name="TError">Type of Error item</typeparam>
    /// </summary>
    public readonly struct Either<TResult, TError>
    {
        private readonly TResult _resultValue;
        private readonly TError _errorValue;
        private readonly bool _success;

        private Either(TResult result)
        {
            _success = true;
            _errorValue = default;
            _resultValue = result;
        }

        private Either(TError error)
        {
            _success = false;
            _errorValue = error;
            _resultValue = default;
        }

        /// <summary>
        ///  Constructs new <see cref="Either{TResult, TError}"/> with the Result part defined.
        /// </summary>
        public static Either<TResult, TError> Result(TResult result)
        {
            return new Either<TResult, TError>(result);
        }

        /// <summary>
        ///  Constructs new <see cref="Either{TResult, TError}"/> with the Error part defined.
        /// </summary>
        public static Either<TResult, TError> Error(TError error)
        {
            return new Either<TResult, TError>(error);
        }

        /// <summary>
        /// Executes result or error function depending on the Either state.
        /// </summary>
        public T Match<T>(Func<TResult, T> resultFunc, Func<TError, T> errorFunc)
        {
            if (resultFunc == null)
            {
                throw new ArgumentNullException(nameof(resultFunc));
            }

            if (errorFunc == null)
            {
                throw new ArgumentNullException(nameof(errorFunc));
            }

            return _success ? resultFunc(_resultValue) : errorFunc(_errorValue);
        }

        /// <summary>
        /// Executes result or error function depending on the Either state.
        /// </summary>
        public T Match<T>(Func<T> resultFunc, Func<T> errorFunc)
        {
            if (resultFunc == null)
            {
                throw new ArgumentNullException(nameof(resultFunc));
            }

            if (errorFunc == null)
            {
                throw new ArgumentNullException(nameof(errorFunc));
            }

            return _success ? resultFunc() : errorFunc();
        }

        /// <summary>
        /// Executes result or error action depending on the Either state.
        /// </summary>
        public void Match(Action<TResult> resultAction, Action<TError> errorAction)
        {
            if (resultAction == null)
            {
                throw new ArgumentNullException(nameof(resultAction));
            }

            if (errorAction == null)
            {
                throw new ArgumentNullException(nameof(errorAction));
            }

            if (_success)
            {
                resultAction(_resultValue);
            }
            else
            {
                errorAction(_errorValue);
            }
        }

        /// <summary>
        /// Executes result or error action depending on the Either state.
        /// </summary>
        public void Match(Action resultAction, Action errorAction)
        {
            if (resultAction == null)
            {
                throw new ArgumentNullException(nameof(resultAction));
            }

            if (errorAction == null)
            {
                throw new ArgumentNullException(nameof(errorAction));
            }

            if (_success)
            {
                resultAction();
            }
            else
            {
                errorAction();
            }
        }

        public TResult ResultOrDefault() => Match(res => res, err => default);
        public TError ErrorOrDefault() => Match(res => default, err => err);
        public TResult ResultOrDefault(TResult defaultValue) => Match(res => res, err => defaultValue);
        public TError ErrorOrDefault(TError defaultValue) => Match(res => defaultValue, err => err);

        public static implicit operator Either<TResult, TError>(TResult result) => Result(result);
        public static implicit operator Either<TResult, TError>(TError error) => Error(error);
    }

    public static class EitherExtensions
    {
        public static Either<TL, TR> ToResult<TL, TR>(this TL result) => Either<TL, TR>.Result(result);
        public static Either<TL, TR> ToError<TL, TR>(this TR error) => Either<TL, TR>.Error(error);
    }
}
