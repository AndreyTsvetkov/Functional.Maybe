using NUnit.Framework;
using System.Threading.Tasks;

namespace Functional.Maybe.Tests
{
	[TestFixture]
	public class MaybeAsyncTests
	{
		[Test]
		public async Task SelectAsyncTest()
		{
			Task<int> Two() => Task.FromResult(2);

			var onePlusTwo = await 1.ToMaybe().SelectAsync(async one => one + (await Two()));

			Assert.AreEqual(3, onePlusTwo.Value);
		}

		[Test]
		public async Task SelectAsyncWithTransformationReturningNullTest()
		{
			Task<string?> GetNull() => Task.FromResult<string?>(null);

			var result = await "a".ToMaybe().SelectAsync(async _ => await GetNull());

			Assert.AreEqual(Maybe<string>.Nothing, result);
		}

		[Test]
		public async Task MatchAsyncWithValueTransformationReturningNullTest()
		{
			Task<string?> GetNull() => Task.FromResult<string?>(null);

			var result = await "a".ToMaybe().MatchAsync(
				async _ => await GetNull(),
				() => Task.FromResult<string?>("a"));

			Assert.IsNull(result);
		}

		[Test]
		public async Task MatchAsyncWithAlternativeFunctionReturningNullTest()
		{
			Task<string?> GetNull() => Task.FromResult<string?>(null);

			var result = await Maybe<string>.Nothing.MatchAsync(
				_ => Task.FromResult<string?>("a"),
				async () => await GetNull());

			Assert.IsNull(result);
		}

		[Test]
		public async Task OrElseWithNullAlternativeValueTest()
		{
			var taskOfMaybe = Task.FromResult(Maybe<string>.Nothing);

			var result = await taskOfMaybe.OrElse(null as string);

			Assert.IsNull(result);
		}

		//bug also OrElseAsync

		[Test]
		public async Task OrElseWithNullAlternativeFuncResultTest()
		{
			var taskOfMaybe = Task.FromResult(Maybe<string>.Nothing);

			var result = await taskOfMaybe.OrElse(() => null as string);

			Assert.IsNull(result);
		}
	}
}
