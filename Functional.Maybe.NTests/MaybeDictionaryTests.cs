using System.Collections.Generic;
using NUnit.Framework;

namespace Functional.Maybe.Tests
{
  class MaybeDictionaryTests
  {
    [Test]
    public void LookupNrtReturnsNothingWhenThereIsNoValueForKey()
    {
      var dictionary = new Dictionary<string, string?>();

      var maybe = dictionary.LookupNrt("a");

      Assert.AreEqual(Maybe<string>.Nothing, maybe);
    }

    [Test]
    public void LookupNrtReturnsNothingWhenValueForKeyIsNull()
    {
      var dictionary = new Dictionary<string, string?>
      {
        ["a"] = null
      };

      var maybe = dictionary.LookupNrt("a");

      Assert.AreEqual(Maybe<string>.Nothing, maybe);
    }

    [Test]
    public void LookupNrtReturnsMaybeOfValueForKeyWhenValueExists()
    {
      var dictionary = new Dictionary<string, string?>
      {
        ["a"] = "b"
      };

      var maybe = dictionary.LookupNrt("a");

      Assert.AreEqual("b".ToMaybe(), maybe);
    }
  }
}
