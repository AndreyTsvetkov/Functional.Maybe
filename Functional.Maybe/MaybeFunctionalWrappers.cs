using System;

namespace Functional.Maybe
{
	/// <summary>
	/// Modifying passed functions making them return Maybe instead of plain type
	/// </summary>
	public static class MaybeFunctionalWrappers
	{
		/// <summary>
		/// Delegate matching usual form of the TryParse methods, such as int.TryParse
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public delegate bool TryGet<in T, TR>(T key, out TR val);

		/// <summary>
		/// Converts a standard tryer function (like int.TryParse, Dictionary.TryGetValue etc.) to a function, returning Maybe
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TK"></typeparam>
		/// <param name="tryer"></param>
		/// <returns></returns>
		public static Func<TK, Maybe<TR>> Wrap<TK, TR>(TryGet<TK, TR> tryer)  
			where TR : notnull => Wrap<TK, TR, TR>(tryer);

		/// <summary>
		/// Converts a standard tryer function (like int.TryParse, Dictionary.TryGetValue etc.) to a function, returning Maybe
		/// </summary>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TK"></typeparam>
		/// <typeparam name="TV"></typeparam>
		/// <param name="tryer"></param>
		/// <returns></returns>
		public static Func<TK, Maybe<TR>> Wrap<TK, TV, TR>(TryGet<TK, TV> tryer)  
			where TR : notnull, TV => (TK arg) => 
			tryer(arg, out var result) ? result.MaybeCast<TV, TR>() : Maybe<TR>.Nothing;

		/// <summary>
		/// Returns a function which calls <paramref name="f"/>, wrapped inside a try-catch clause with <typeparamref name="TEx"/> catched. 
		/// That new function returns Nothing in the case of the <typeparamref name="TEx"/> thrown inside <paramref name="f"/>, otherwise it returns the f-result as Maybe
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TEx"></typeparam>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Func<TA, Maybe<TR>> Catcher<TA, TR, TEx>(Func<TA, TR> f)
			where TEx : Exception where TR : notnull
		{
			return (TA arg) =>
			{
				try
				{
					return f(arg).ToMaybe();
				}
				catch (TEx)
				{
					return default;
				}
			};
		}

		/// <summary>
		/// Returns a function which calls <paramref name="f"/>, wrapped inside a try-catch clause with <typeparamref name="TEx"/> catched. 
		/// That new function returns Nothing in the case of the <typeparamref name="TEx"/> thrown inside <paramref name="f"/>, otherwise it returns the f-result as Maybe
		/// </summary>
		/// <typeparam name="TA"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <typeparam name="TEx"></typeparam>
		/// <typeparam name="TF">Func return type</typeparam>
		/// <param name="f"></param>
		/// <returns></returns>
		public static Func<TA, Maybe<TR>> Catcher<TA, TF, TR, TEx>(Func<TA, TF> f) 
			where TEx : Exception 
			where TR : notnull, TF => (TA arg) =>
		{
			try
			{
				return f(arg).MaybeCast<TF, TR>();
			}
			catch (TEx)
			{
				return default;
			}
		};

  }
}