using System;
// ReSharper disable MemberCanBePrivate.Global

namespace Functional.Maybe;

/// <summary>
/// Providing necessary methods to enable linq syntax with Maybes themselves
/// </summary>
public static class MaybeLinq
{
	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it and returns the result as Maybe, otherwise returns Nothing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <returns></returns>
	public static Maybe<TResult> Select<T, TResult>(this Maybe<T> a, Func<T, TResult> fn)
	{
		if (a.HasValue)
		{
			var result = fn(a.Value);

			return result != null
				? new Maybe<TResult>(result)
				: default;
		}
		else
			return default;
	}
	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it and returns the result, otherwise returns <paramref name="else"/>()
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <param name="else"></param>
	/// <returns></returns>
	public static TResult SelectOrElse<T, TResult>(this Maybe<T> a, Func<T, TResult> fn, Func<TResult> @else) => 
		a.HasValue ? fn(a.Value) : @else();
	/// <summary>
	/// If <paramref name="a"/> has value, and it fulfills the <paramref name="predicate"/>, returns <paramref name="a"/>, otherwise returns Nothing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a"></param>
	/// <param name="predicate"></param>
	/// <returns></returns>
	public static Maybe<T> Where<T>(this Maybe<T> a, Func<T, bool> predicate) => 
		a.HasValue && predicate(a.Value) ? a : default;
	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it and returns, otherwise returns Nothing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <returns></returns>
	public static Maybe<TR> SelectMany<T, TR>(this Maybe<T> a, Func<T, Maybe<TR>> fn) => 
		a.HasValue ? fn(a.Value) : default;

	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it, and if the result also has value, calls <paramref name="composer"/> on both values 
	/// (original and fn-call-resulted), and returns the <paramref name="composer"/>-call result, wrapped in Maybe. Otherwise returns nothing.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TTempResult"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <param name="composer"></param>
	/// <returns></returns>
	public static Maybe<TResult> SelectMany<T, TTempResult, TResult>(this Maybe<T> a, Func<T, Maybe<TTempResult>> fn, Func<T, TTempResult, TResult> composer) => 
		a.SelectMany(x => fn(x).SelectMany(y => composer(x, y).ToMaybe()));

	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it and returns, otherwise returns Nothing. Alias for SelectMany.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <returns></returns>
	public static Maybe<TR> SelectMaybe<T, TR>(this Maybe<T> a, Func<T, Maybe<TR>> fn) =>
		a.SelectMany(fn);

	/// <summary>
	/// If <paramref name="a"/> has value, applies <paramref name="fn"/> to it, and if the result also has value, calls <paramref name="composer"/> on both values 
	/// (original and fn-call-resulted), and returns the <paramref name="composer"/>-call result, wrapped in Maybe. Otherwise returns nothing.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TTempResult"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="a"></param>
	/// <param name="fn"></param>
	/// <param name="composer"></param>
	/// <returns></returns>
	public static Maybe<TResult> SelectMaybe<T, TTempResult, TResult>(this Maybe<T> a, Func<T, Maybe<TTempResult>> fn, Func<T, TTempResult, TResult> composer) =>
		a.SelectMany(fn, composer);
}