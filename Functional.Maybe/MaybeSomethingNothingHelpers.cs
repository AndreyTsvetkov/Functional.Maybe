using System.Runtime.CompilerServices;

namespace Functional.Maybe;

/// <summary>
/// IsSomething, IsNothing and shorthands to create typed Nothing of correct type
/// </summary>
public static class MaybeSomethingNothingHelpers
{
	/// <summary>
	/// Has a value inside
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsSomething<T>(this Maybe<T> a) => a.HasValue;

	/// <summary>
	/// Has no value inside
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="a"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing<T>(this Maybe<T> a) => !a.IsSomething();

	/// <summary>
	/// Создает "ничто" такого же типа, как исходный объект
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="_"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<T> NothingOf<T>(this Maybe<T> _) => default;

	/// <summary>
	/// Создает "ничто" такого же типа, как исходный объект
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="_"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Maybe<T> NothingOf<T>(this T _) => default;
}