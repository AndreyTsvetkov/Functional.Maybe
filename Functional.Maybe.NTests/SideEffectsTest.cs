using NUnit.Framework;

namespace Functional.Maybe.NTests;

[TestFixture]
public class SideEffectsTest
{
	[Test]
	public void DoOnNothing_DoesNothing()
	{
		var target = "unchanged";
		Maybe<string>.Nothing.Do(_ => target = "changed");
		Assert.AreEqual("unchanged", target);
	}
	[Test]
	public void DoOnSomething_DoesSomething()
	{
		var target = "unchanged";
		"changed".ToMaybe().Do(_ => target = _);
		Assert.AreEqual("changed", target);
	}

	[Test]
	public void MatchOnNothing_MatchesNothing()
	{
		var target1 = "unchanged";
		var target2 = "unchanged";
		Maybe<string>.Nothing.Match(_ => target1 = "changed", () => target2 = "changed");
		Assert.AreEqual("unchanged", target1);
		Assert.AreEqual("changed", target2);
	}
	[Test]
	public void MatchOnSomething_MatchesSomething()
	{
		var target1 = "unchanged";
		var target2 = "unchanged";
		"κατι".ToMaybe().Match(_ => target1 = "changed", () => target2 = "changed");
		Assert.AreEqual("changed", target1);
		Assert.AreEqual("unchanged", target2);
	}
}