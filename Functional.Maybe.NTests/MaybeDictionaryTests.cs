using System.Collections.Generic;
using NUnit.Framework;

namespace Functional.Maybe.Tests
{
	class MaybeDictionaryTests
	{
		[Test]
		public void LookupReturnsNothingWhenThereIsNoNrtValueForKey()
		{
			var dictionary = new Dictionary<string, string?>();

			var maybe = dictionary.Lookup<string, string?, string>("a");

			Assert.AreEqual(Maybe<string>.Nothing, maybe);
		}

		[Test]
		public void LookupReturnsNothingWhenNrtValueForKeyIsNull()
		{
			var dictionary = new Dictionary<string, string?>
			{
				["a"] = null
			};

			var maybe = dictionary.Lookup<string, string?, string>("a");

			Assert.AreEqual(Maybe<string>.Nothing, maybe);
		}

		[Test]
		public void LookupReturnsMaybeOfNrtValueForKeyWhenValueExists()
		{
			var dictionary = new Dictionary<string, string?>
			{
				["a"] = "b"
			};

			var maybe = dictionary.Lookup<string, string?, string>("a");

			Assert.AreEqual("b".ToMaybe(), maybe);
		}
	}
}
