using System.Collections.Generic;

namespace Functional.Maybe
{
	public static class MaybeDictionary
	{
		/// <summary>
		/// Tries to get value from Dictionary safely
		/// </summary>
		/// <typeparam name="TK"></typeparam>
		/// <typeparam name="T"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Maybe<T> Lookup<TK, T>(this IDictionary<TK, T> dictionary, TK key) 
			where T : notnull
			where TK : notnull => Lookup<TK, T, T>(dictionary!, key);

		/// <summary>
		/// Tries to get value from Dictionary safely
		/// </summary>
		/// <typeparam name="TK">dictionary key type</typeparam>
		/// <typeparam name="TV">dictionary value type</typeparam>
		/// <typeparam name="TR">returned maybe type</typeparam>
		/// <param name="dictionary"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static Maybe<TR> Lookup<TK, TV, TR>(this IDictionary<TK, TV> dictionary, TK key) 
			where TR : notnull, TV
			where TK : notnull
		{
			var getter = MaybeFunctionalWrappers.Wrap<TK, TV, TR>(dictionary.TryGetValue);
			return getter(key);
		}
	}
}