using System;
using System.Collections.Generic;
// ReSharper disable MemberCanBePrivate.Global

namespace Functional.Maybe
{
	/// <summary>
	/// Composing two maybe value to one, such operations, as one.Or(another)
	/// </summary>
	public static class MaybeCompositions
	{
		/// <summary>
		/// Returns <paramref name="a"/> if its Value exists or returns <paramref name="b"/>, wrapped as Maybe
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Maybe<T> Or<T>(this Maybe<T> a, T b)
		{
			if (a.IsSomething())
				return a;
			return b.ToMaybe();
		}

		/// <summary>
		/// Returns <paramref name="a"/> if its Value exists or returns <paramref name="b"/>()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Maybe<T> Or<T>(this Maybe<T> a, Func<Maybe<T>> b)
		{
			if (a.IsSomething())
				return a;
			return b();
		}

		/// <summary>
		/// Returns <paramref name="a"/> if its Value exists or returns <paramref name="b"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Maybe<T> Or<T>(this Maybe<T> a, Maybe<T> b)
		{
			if (a.IsSomething())
				return a;
			return b;
		}

		/// <summary>
		/// Returns <paramref name="b"/> if <paramref name="a"/> has value, otherwise <see cref="Maybe&lt;T&gt;.Nothing"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Maybe<T2> Compose<T, T2>(this Maybe<T> a, Maybe<T2> b)
		{
			if (a.IsNothing())
				return Maybe<T2>.Nothing;
			return b;
		}

		/// <summary>
		/// Collapses nested maybes to a flat one
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static Maybe<T> Collapse<T>(this Maybe<Maybe<T>> t)
		{
			// using implicit cast
			return t;
		}

		/// <summary>
		/// Flattens a recursive Maybe structure into IEnumerable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="maybe"></param>
		/// <param name="parentSelector"></param>
		/// <example>
		/// Having { a: 1, parent: { a: 2, parent: { a: 3, parent: Nothing } } } 
		/// We can flatten it to 
		/// [
		///		{ a: 1, parent: { a: 2, parent: { a: 3, parent: Nothing } } }, 
		///		{ a: 2, parent: { a: 3, parent: Nothing } } , 
		///		{ a: 3, parent: Nothing } 
		///	]
		/// </example>
		/// <returns></returns>
		public static IEnumerable<T> Flatten<T>(this Maybe<T> maybe, Func<T, Maybe<T>> parentSelector)
		{
			return maybe.FlattenSelect(parentSelector, x => x);
		}

		private static IEnumerable<TFlatten> FlattenSelect<TMaybe, TFlatten>(this Maybe<TMaybe> maybe, Func<TMaybe, Maybe<TMaybe>> parentSelector, Func<TMaybe, TFlatten> flattenSelector)
		{
			return maybe.Flatten(parentSelector, new List<TFlatten>(), flattenSelector);
		}

		private static IEnumerable<TFlatten> Flatten<TMaybe, TFlatten>(this Maybe<TMaybe> maybe, Func<TMaybe, Maybe<TMaybe>> parentSelector, List<TFlatten> acc, Func<TMaybe, TFlatten> flattenSelector)
		{
			while (true)
			{
				if (maybe.IsNothing())
					return acc;

				acc.Add(flattenSelector(maybe.Value));
				maybe = parentSelector(maybe.Value);
			}
		}
	}
}