using System;

namespace Functional.Maybe
{
	/// <summary>
	/// Fluent exts for unwrapping values from the Maybe
	/// </summary>
	public static class MaybeReturns
	{
		/// <summary>
		/// Returns <paramref name="a"/>.Value.ToString() or <paramref name="default"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static string ReturnToString<T>(this Maybe<T> a, string @default) where T : notnull
		{
			return a.HasValue ? a.Value.ToString() : @default;
		}

		/// <summary>
		/// Returns <paramref name="a"/>.Value or throws <paramref name="e"/>()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public static T OrElse<T>(this Maybe<T> a, Func<Exception> e)  
			where T : notnull
		{
			if (a.IsNothing())
			{
				throw e();
			}
			return a.Value;
		}

		/// <summary>
		/// Returns <paramref name="a"/>.Value or returns <paramref name="default"/>()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="a"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static TResult OrElse<T, TResult>(this Maybe<T> a, Func<TResult> @default)  
			where T : notnull, TResult =>
			a.HasValue ? a.Value : @default();

		/// <summary>
		/// Returns <paramref name="a"/>.Value or returns default(<typeparamref name="T"/>)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static T? OrElseDefault<T>(this Maybe<T> a) where T : notnull =>
			a.HasValue ? a.Value : default;

		/// <summary>
		/// Returns <paramref name="a"/>.Value or returns <paramref name="default"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="a"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static TResult OrElse<T, TResult>(this Maybe<T> a, TResult @default) 
			where T : notnull, TResult =>
			a.HasValue ? a.Value : @default;
	}
}