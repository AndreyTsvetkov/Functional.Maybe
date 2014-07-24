using System;

namespace Functional.Maybe
{
	/// <summary>
	/// Ternary logic with Maybe&lt;bool&gt; and combining T and bool to a Maybe value
	/// </summary>
	public static class MaybeBoolean
	{
		/// <summary>
		/// If <paramref name="condition"/> returns <paramref name="f"/>() as Maybe, otherwise Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="condition"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Maybe<T> Then<T>(this bool condition, Func<T> f)
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			return condition ? f().ToMaybe() : Maybe<T>.Nothing;
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// If <paramref name="condition"/> returns <paramref name="t"/> as Maybe, otherwise Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="condition"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static Maybe<T> Then<T>(this bool condition, T t)
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			return condition ? t.ToMaybe() : Maybe<T>.Nothing;
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		/// <summary>
		/// Calls <paramref name="fn"/> if <paramref name="m"/> is true.ToMaybe()
		/// </summary>
		/// <param name="m"></param>
		/// <param name="fn"></param>
		public static void DoWhenTrue(this Maybe<bool> m, Action fn)
		{
			if (m.HasValue && m.Value)
				fn();
		}
	}
}