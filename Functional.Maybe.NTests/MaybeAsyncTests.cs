using System.Threading.Tasks;
using NUnit.Framework;

namespace Functional.Maybe.NTests;

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
}