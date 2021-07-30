using System;
using System.Collections.Generic;
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
		public static Maybe<T> FirstMaybe<T>(this IEnumerable<T?> items) where T : notnull => 
			FirstMaybeNrt(items, arg => true);

    /// <summary>
    /// First item matching <paramref name="predicate"/> or Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> FirstMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate) where T : notnull => 
      FirstMaybeNrt(items, predicate!);

    /// <summary>
    /// First item matching <paramref name="predicate"/> or Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> FirstMaybeNrt<T>(this IEnumerable<T?> items, Func<T?, bool> predicate) where T : notnull
    {
      foreach (var item in items)
      {
        if (predicate(item))
        {
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
		public static Maybe<T> SingleMaybe<T>(this IEnumerable<T?> items) where T : notnull => 
			SingleMaybeNrt(items, arg => true);

    /// <summary>
    /// Single item matching <paramref name="predicate"/> or Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> SingleMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate) where T : notnull
      => SingleMaybeNrt(items, predicate!);

		/// <summary>
		/// Single item matching <paramref name="predicate"/> or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static Maybe<T> SingleMaybeNrt<T>(this IEnumerable<T?> items, Func<T?, bool> predicate) where T : notnull
		{
			var result = default(T);
			var count = 0;
			foreach (var element in items)
			{
				if (predicate(element))
				{
					result = element;
					count++;
					if (count > 1)
					{
						return default;
					}
				}
			}

			switch (count)
			{
				case 0:
					return default;
				case 1:
					return result.ToMaybe();
			}

			return default;
		}

    /// <summary>
		/// Last item or Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
	  public static Maybe<T> LastMaybe<T>(this IEnumerable<T?> items) where T : notnull => 
  		LastMaybeNrt(items, arg => true);

    /// <summary>
    /// Last item matching <paramref name="predicate"/> or Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> LastMaybe<T>(this IEnumerable<T> items, Func<T, bool> predicate) where T : notnull
      => LastMaybeNrt(items, predicate!);

    /// <summary>
    /// Last item matching <paramref name="predicate"/> or Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Maybe<T> LastMaybeNrt<T>(this IEnumerable<T?> items, Func<T?, bool> predicate) where T : notnull
    {
	    var result = default(T);
	    var found = false;
	    foreach (var element in items)
	    {
		    if (predicate(element))
		    {
			    result = element;
			    found = true;
		    }
	    }

	    return found ? result.ToMaybe() : default;
    }

    /// <summary>
    /// Returns the value of <paramref name="maybeCollection"/> if exists orlse an empty collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maybeCollection"></param>
    /// <returns></returns>
    public static IEnumerable<T> FromMaybe<T>(this Maybe<IEnumerable<T>> maybeCollection) =>
      FromMaybeNrt(maybeCollection!)!;
    
    /// <summary>
		/// Returns the value of <paramref name="maybeCollection"/> if exists orlse an empty collection
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybeCollection"></param>
		/// <returns></returns>
		public static IEnumerable<T?> FromMaybeNrt<T>(this Maybe<IEnumerable<T?>> maybeCollection) =>
			maybeCollection.HasValue ? maybeCollection.Value : Enumerable.Empty<T?>();

		/// <summary>
		/// For each items that has value, applies <paramref name="selector"/> to it and wraps back as Maybe, for each otherwise remains Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="maybes"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public static IEnumerable<Maybe<TResult>> Select<T, TResult>(this IEnumerable<Maybe<T>> maybes, Func<T, TResult?> selector) 
      where T : notnull where TResult : notnull =>
			maybes.Select(maybe => maybe.Select(selector));

		/// <summary>
		/// If all the items have value, unwraps all and returns the whole sequence of <typeparamref name="T"/>, wrapping the whole as Maybe, otherwise returns Nothing 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybes"></param>
		/// <returns></returns>
		public static Maybe<IEnumerable<T>> WholeSequenceOfValues<T>(this IEnumerable<Maybe<T>> maybes) where T : notnull
		{
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
		public static IEnumerable<T> WhereValueExist<T>(this IEnumerable<Maybe<T>> maybes) where T : notnull => 
			SelectWhereValueExist(maybes, m => m);

		/// <summary>
		/// Filters out all the Nothings, unwrapping the rest to just type <typeparamref name="T"/> and then applies <paramref name="fn"/> to each
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="maybes"></param>
		/// <param name="fn"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> SelectWhereValueExist<T, TResult>(this IEnumerable<Maybe<T>> maybes, Func<T, TResult> fn) where T : notnull =>
			from maybe in maybes
			where maybe.HasValue
			select fn(maybe.Value);

		/// <summary>
		/// Checks if any item is Nothing 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybes"></param>
		/// <returns></returns>
		public static bool AnyNothing<T>(this IEnumerable<Maybe<T>> maybes) where T : notnull =>
			maybes.Any(m => !m.HasValue);

		//bug UT and maybe an Nrt version?
		/// <summary>
		/// If ALL calls to <paramref name="pred"/> returned a value, filters out the <paramref name="xs"/> based on that values, otherwise returns Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xs"></param>
		/// <param name="pred"></param>
		/// <returns></returns>
		public static Maybe<IEnumerable<T>> WhereAll<T>(this IEnumerable<T> xs, Func<T, Maybe<bool>> pred)
		{
			var l = new List<T>();
			foreach (var x in xs)
			{
				var r = pred(x);
				if (!r.HasValue)
					return default;
				if (r.Value)
					l.Add(x);
			}
			return new Maybe<IEnumerable<T>>(l);
		}

		//bug UT and NRT version?
		/// <summary>
		/// Filters out <paramref name="xs"/> based on <paramref name="pred"/> resuls; Nothing considered as False
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xs"></param>
		/// <param name="pred"></param>
		/// <returns></returns>
		public static IEnumerable<T> Where<T>(this IEnumerable<T> xs, Func<T, Maybe<bool>> pred) =>
			from x in xs
			let b = pred(x)
			where b.HasValue && b.Value
			select x;

		/// <summary>
		/// Combines all exisiting values into single IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this Maybe<T> @this, params Maybe<T>[] others) where T : notnull =>
			@this.Union(others.WhereValueExist());

		/// <summary>
		/// Combines the current value, if exists, with passed IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this Maybe<T> @this, IEnumerable<T> others) where T : notnull =>
			@this.ToEnumerable().Union(others);

		/// <summary>
		/// Combines the current value, if exists, with passed IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static IEnumerable<T> Union<T>(this IEnumerable<T> @these, Maybe<T> other) where T : notnull =>
			@these.Union(other.ToEnumerable());
	}
}