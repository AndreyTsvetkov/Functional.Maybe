using System;
using NUnit.Framework;

namespace Functional.Maybe.NTests
{
  class MaybeFunctionalWrappersTests
  {
    [Test]
    public void METHOD1() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, Exception>(s => null);

      var result = catcher("a");

      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

    [Test]
    public void METHOD2() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, Exception>(s => 
          throw new Exception());

      var result = catcher("a");

      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

    [Test]
    public void METHOD3() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, InvalidCastException>(
          s => throw new Exception());

      Assert.Throws<Exception>(() => catcher("a"));
    }
  }
}
