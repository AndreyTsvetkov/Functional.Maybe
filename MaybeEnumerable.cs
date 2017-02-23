using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Functional.Maybe
{
	/// <summary>
	/// Integration with Enumerable's LINQ (such as .FirstMaybe()) and all kinds of cross-use of IEnumerables and Maybes
	/// </summary>
	public static class MaybeEnumerable
	{
		/// <summary>
		/// First item or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> items)
		{
			Contract.Requires(items != null);
			return FirstMaybe(items, arg => true);
		}

        /// <summary>
        /// First item matching <paramref name="predicate"/> or Nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate)
	    {
	        Contract.Requires(items != null);
	        Contract.Requires(predicate != null);

	        foreach(var item in items) {
	            if(predicate(item)) {
	                return item.ToMaybe();
	            }
	        }
	        return Maybe<T>.Nothing;
	    }

	    /// <summary>
		/// Single item or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static Maybe<T> SingleMaybe<T>(this IEnumerable<T> items)
		{
			Contract.Requires(items != null);
			return SingleMaybe(items, arg => true);
		}

		/// <summary>
		/// Single item matching <paramref name="predicate"/> or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Maybe<T> SingleMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			Contract.Requires(items != null);
			Contract.Requires(predicate != null);

            var result = default(T);
            var count = 0;
            foreach(var element in items) {
                if(predicate(element)) {
                    result = element;
                    count++;
                    if(count > 1)
                        return Maybe<T>.Nothing;
                }
            }
            switch(count) {
                case 0: return Maybe<T>.Nothing;
                case 1: return result.ToMaybe();
            }
            return Maybe<T>.Nothing;
        }

		/// <summary>
		/// Last item or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static Maybe<T> LastMaybe<T>(this IEnumerable<T> items)
		{
			Contract.Requires(items != null);
			return LastMaybe(items, arg => true);
		}

	    /// <summary>
	    /// Last item matching <paramref name="predicate"/> or Nothing
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="items"></param>
	    /// <param name="predicate"></param>
	    /// <returns></returns>
	    public static Maybe<T> LastMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate)
	    {
	        Contract.Requires(items != null);
	        Contract.Requires(predicate != null);

	        var result = default(T);
	        var found = false;
	        foreach(var element in items) {
	            if(predicate(element)) {
	                result = element;
	                found = true;
	            }
	        }
	        if(found)
	            return result.ToMaybe();
	        return Maybe<T>.Nothing;
	    }

	    /// <summary>
		/// Returns the value of <paramref name="maybeCollection"/> if exists orlse an empty collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybeCollection"></param>
		/// <returns></returns>
		public static IEnumerable<T> FromMaybe<T>(this Maybe<IEnumerable<T>> maybeCollection)
		{
			return maybeCollection.HasValue ? maybeCollection.Value : Enumerable.Empty<T>();
		}

		/// <summary>
		/// For each items that has value, applies <paramref name="selector"/> to it and wraps back as Maybe, for each otherwise remains Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="maybes"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<Maybe<TResult>> Select<T, TResult>(this IEnumerable<Maybe<T>> maybes, Func<T, TResult> selector)
		{
			Contract.Requires(maybes != null);
			return maybes.Select(maybe => maybe.Select(selector));
		}

		/// <summary>
		/// If all the items have value, unwraps all and returns the whole sequence of <typeparamref name="T"/>, wrapping the whole as Maybe, otherwise returns Nothing 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybes"></param>
		/// <returns></returns>
		public static Maybe<IEnumerable<T>> WholeSequenceOfValues<T>(this IEnumerable<Maybe<T>> maybes)
		{
			Contract.Requires(maybes != null);
			var forced = maybes.ToArray();
			// there has got to be a better way to do this
			if (forced.AnyNothing())
				return Maybe<IEnumerable<T>>.Nothing;

			return forced.Select(m => m.Value).ToMaybe();
		}

		/// <summary>
		/// Filters out all the Nothings, unwrapping the rest to just type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybes"></param>
		/// <returns></returns>
		public static IEnumerable<T> WhereValueExist<T>(this IEnumerable<Maybe<T>> maybes)
		{
			Contract.Requires(maybes != null);
			return SelectWhereValueExist(maybes, m => m);
		}

		/// <summary>
		/// Filters out all the Nothings, unwrapping the rest to just type <typeparamref name="T"/> and then applies <paramref name="fn"/> to each
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="maybes"></param>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SelectWhereValueExist<T, TResult>(this IEnumerable<Maybe<T>> maybes, Func<T, TResult> fn)
		{
			Contract.Requires(maybes != null);
			return from maybe in maybes
			       where maybe.HasValue
			       select fn(maybe.Value);
		}

		/// <summary>
		/// Checks if any item is Nothing 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybes"></param>
		/// <returns></returns>
		public static bool AnyNothing<T>(this IEnumerable<Maybe<T>> maybes)
		{
			Contract.Requires(maybes != null);
			return maybes.Any(m => !m.HasValue);
		}

		/// <summary>
		/// If ALL calls to <paramref name="pred"/> returned a value, filters out the <paramref name="xs"/> based on that values, otherwise returns Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xs"></param>
		/// <param name="pred"></param>
		/// <returns></returns>
		public static Maybe<IEnumerable<T>> WhereAll<T>(this IEnumerable<T> xs, Func<T, Maybe<bool>> pred)
		{
			Contract.Requires(xs != null);
			var l = new List<T>();
			foreach (var x in xs)
			{
				var r = pred(x);
				if (!r.HasValue)
					return Maybe<IEnumerable<T>>.Nothing;
				if (r.Value)
					l.Add(x);
			}
			return new Maybe<IEnumerable<T>>(l);
		}

		/// <summary>
		/// Filters out <paramref name="xs"/> based on <paramref name="pred"/> resuls; Nothing considered as False
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xs"></param>
		/// <param name="pred"></param>
		/// <returns></returns>
		public static IEnumerable<T> Where<T>(this IEnumerable<T> xs, Func<T, Maybe<bool>> pred)
		{
			Contract.Requires(xs != null);
			return from x in xs
			       let b = pred(x)
			       where b.HasValue && b.Value
			       select x;
		}

		/// <summary>
		/// Combines all exisiting values into single IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this Maybe<T> @this, params Maybe<T>[] others)
		{
			return @this.Union(others.WhereValueExist());
		}

		/// <summary>
		/// Combines the current value, if exists, with passed IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this Maybe<T> @this, IEnumerable<T> others)
		{
			Contract.Requires(others != null);

			return @this.ToEnumerable().Union(others);
		}

		/// <summary>
		/// Combines the current value, if exists, with passed IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this IEnumerable<T> @these, Maybe<T> other)
		{
			Contract.Requires(@these != null);

			return @these.Union(other.ToEnumerable());
		}
	}
}