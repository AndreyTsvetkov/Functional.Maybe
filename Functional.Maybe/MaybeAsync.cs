using System;
using System.Threading.Tasks;
#pragma warning disable 618

namespace Functional.Maybe
{
	public static class MaybeAsync
	{
		/// <summary>
		/// Flips Maybe and Task: instead of having Maybe{Task{T}} (as in case of Select) we get Task{Maybe{T}} and have possibility to await on it.
		/// </summary>
		/// <typeparam name="T">source type</typeparam>
		/// <typeparam name="TR">async result type</typeparam>
		/// <param name="this">maybe to map</param>
		/// <param name="res">async mapper</param>
		/// <returns>Task of Maybe of TR</returns>
		public static async Task<Maybe<TR>> SelectAsync<T, TR>(this Maybe<T> @this, Func<T, Task<TR>> res) => @this.HasValue
			? (await res(@this.Value)).ToMaybe()
			: (default);
		public static async Task<T> OrElseAsync<T>(this Task<Maybe<T>> @this, Func<Task<T>> orElse)
		{
			var res = await @this;
			return res.HasValue ? res.Value : await orElse();
		}
		public static async Task<T> OrElse<T>(this Task<Maybe<T>> @this, T orElse)
		{
			var res = await @this;
			return res.HasValue ? res.Value : orElse;
		}
		public static async Task<T> OrElse<T>(this Task<Maybe<T>> @this, Func<T> orElse)
		{
			var res = await @this;
			return res.HasValue ? res.Value : orElse();
		}
	}
}
