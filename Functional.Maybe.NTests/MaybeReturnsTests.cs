using NUnit.Framework;

namespace Functional.Maybe.Tests
{
	[TestFixture]
	public class MaybeReturnsTests
	{
		[Test]
		public void OrElseTestWhenMaybeIsNothingAndOtherIsNotNull()
		{
			Assert.AreEqual("a", Maybe<string>.Nothing.OrElse("a"));
			Assert.AreEqual("a", Maybe<string>.Nothing.OrElse(() => "a"));
		}

		[Test]
		public void OrElseTestWhenMaybeIsNothingAndOtherIsNull()
		{
			Assert.IsNull(Maybe<string>.Nothing.OrElse(null as string));
			Assert.IsNull(Maybe<string>.Nothing.OrElse(() => null as string));
		}

		[Test]
		public void OrTestWithDefaultValueWhenMaybeIsNothingAndOtherIsNull()
		{
			Assert.AreEqual(Maybe<string>.Nothing, Maybe<string>.Nothing.Or(null as string));
		}
	}
}