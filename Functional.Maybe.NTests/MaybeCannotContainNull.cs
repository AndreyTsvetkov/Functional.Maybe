using NUnit.Framework;

namespace Functional.Maybe.Tests
{
	[TestFixture]
	public class MaybeCannotContainNull
	{
		private class User
		{
			public string? Name { get; set; }
		}

		[Test]
		public void WhenSelectingNull_GettingNothing()
		{
			var user = new User { Name = null };

			var maybeUser = user.ToMaybe();

			Assert.AreEqual(Maybe<string>.Nothing, maybeUser.Select(_ => _.Name));

		}
	}
}
