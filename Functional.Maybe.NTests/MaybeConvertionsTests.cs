using NUnit.Framework;

namespace Functional.Maybe.Tests
{
  class MaybeConvertionsTests
  {
    [Test]
    public void MaybeCastConvertsValueToItsMaybe()
    {
      var result = "a".MaybeCast<string, string>();
      Assert.AreEqual("a".ToMaybe(), result);
    }

    [Test]
    public void MaybeCastConvertsValueOfNrtToItsMaybe()
    {
      var result = "a".MaybeCast<string?, string>();
      Assert.AreEqual("a".ToMaybe(), result);
    }

    [Test]
    public void MaybeCastConvertsNonCastableValueOfNrtToNothing()
    {
      var result = "a".MaybeCast<object?, int>();
      Assert.AreEqual(Maybe<int>.Nothing, result);
    }

    [Test]
    public void MaybeCastConvertsNullToNothing()
    {
      var result = (null as string).MaybeCast<string?, string>();
      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

  }
}
