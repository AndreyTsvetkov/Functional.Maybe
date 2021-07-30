using System.Collections.Generic;
using NUnit.Framework;

namespace Functional.Maybe.NTests
{
  class MaybeDictionaryNrtTests
  {
    [Test]
    public void METHOD1() //bug
    {
      var dictionary = new Dictionary<string, string?>();

      var maybe = dictionary.LookupNrt("a");

      Assert.AreEqual(Maybe<string>.Nothing, maybe);
    }

    [Test]
    public void METHOD2() //bug
    {
      var dictionary = new Dictionary<string, string?>
      {
        ["a"] = null
      };

      var maybe = dictionary.LookupNrt("a");

      Assert.AreEqual(Maybe<string>.Nothing, maybe);
    }

    [Test]
    public void METHOD3() //bug
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
