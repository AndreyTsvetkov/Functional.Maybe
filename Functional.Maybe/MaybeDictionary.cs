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
		{
			var getter = MaybeFunctionalWrappers.Wrap<TK, T>(dictionary.TryGetValue);
			return getter(key);
		} //bug check this!

    /// <summary>
    /// Tries to get value from Dictionary safely
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Maybe<T> LookupNrt<TK, T>(this IDictionary<TK, T?> dictionary, TK key) 
      where T : notnull
      where TK : notnull
    {
      var getter = MaybeFunctionalWrappers.WrapNrt<TK, T>(dictionary.TryGetValue);
      return getter(key);
    } //bug check this!

	}
}