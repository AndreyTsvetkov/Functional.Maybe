using System;
using System.Collections.Generic;

namespace Functional.Maybe
{
	/// <summary>
	/// Fluent exts for converting the values of Maybe to/from lists, nullables; casting and upshifting
	/// </summary>
	public static class MaybeConvertions
	{
		/// <summary>
		/// If <paramref name="a"/>.Value exists and can be successfully casted to <typeparamref name="TB"/>, returns the casted one, wrapped as Maybe&lt;TB&gt;, otherwise Nothing
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TB"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Maybe<TB> Cast<TA, TB>(this Maybe<TA> a) where TB : class
		{
			return from m in a
			       let t = m as TB
			       where t != null
			       select t;
		}

		/// <summary>
		/// If <paramref name="a"/> can be successfully casted to <typeparamref name="TR"/>, returns the casted one, wrapped as Maybe&lt;TR&gt;, otherwise Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Maybe<TR> MaybeCast<T, TR>(this T a) where TR : T
		{
			return MaybeFunctionalWrappers.Catcher<T, TR, InvalidCastException>(o => (TR)o)(a);
		}

		/// <summary>
		/// If <paramref name="a"/>.Value is present, returns a list of that single value, otherwise an empty list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static IEnumerable<T> ReturnList<T>(this Maybe<T> a)
		{
			if (a.IsSomething())
				yield return a.Value;
		}

		/// <summary>
		/// If <paramref name="xs"/> contains any items, returns first one wrapped as Maybe, elsewhere returns Nothing
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xs"></param>
		/// <returns></returns>
		public static Maybe<T> ToMaybeFromList<T>(this IEnumerable<T> xs)
		{
			return xs.FirstMaybe();
		}

		/// <summary>
		/// Converts Maybe to corresponding Nullable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static T? ToNullable<T>(this Maybe<T> a) where T : struct
		{
			return a.IsSomething() ? a.Value : new T?();
		}

		/// <summary>
		/// Converts Nullable to corresponding Maybe
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Maybe<T> ToMaybe<T>(this T? a) where T : struct
		{
			if (!a.HasValue)
				return Maybe<T>.Nothing;
			return a.Value.ToMaybe();
		}

		/// <summary>
		/// Returns <paramref name="a"/> wrapped as Maybe
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="a"></param>
		/// <returns></returns>
		public static Maybe<T> ToMaybe<T>(this T a)
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if (a == null)
				return Maybe<T>.Nothing;
			return new Maybe<T>(a);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}
	}
}