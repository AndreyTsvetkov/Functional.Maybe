using NUnit.Framework;

namespace Functional.Maybe.Tests
{
  class MaybeConvertionsTests
  {
    [Test]
    public void METHOD1() //bug
    {
      var result = "a".MaybeCast<string, string>();
      Assert.AreEqual("a".ToMaybe(), result);
    }

    [Test]
    public void METHOD2() //bug
    {
      var result = (null as string).MaybeCast<string?, string>();
      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

    [Test]
    public void METHOD3() //bug
    {
      var result = "a".MaybeCast<string?, string>();
      Assert.AreEqual("a".ToMaybe(), result);
    }
  }
}
