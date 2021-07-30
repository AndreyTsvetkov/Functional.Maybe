using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Functional.Maybe.Tests
{
  class MaybeFunctionalWrappersTests
  {
    [Test]
    public void METHOD1() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, Exception>(_ => null);

      var result = catcher("a");

      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

    [Test]
    public void METHOD2() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, Exception>(
          _ => throw new Exception());

      var result = catcher("a");

      Assert.AreEqual(Maybe<string>.Nothing, result);
    }

    [Test]
    public void METHOD3() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string, string, InvalidCastException>(
          _ => throw new Exception());

      Assert.Throws<Exception>(() => catcher("a"));
    }

    [Test]
    public void METHOD4() //bug
    {
      var catcher = MaybeFunctionalWrappers
        .CatcherNrt<string?, string, InvalidCastException>(
          _ => throw new Exception());

      Assert.Throws<Exception>(() => catcher(null));
    }

    [Test]
    public void METHOD5() //bug
    {
      MaybeFunctionalWrappers.TryGet<string?, int> tryParse = int.TryParse;
      var wrapped = MaybeFunctionalWrappers.Wrap(tryParse);

      var result = wrapped(null);

      Assert.AreEqual(Maybe<int>.Nothing, result);
    }

    [Test]
    public void METHOD6() //bug
    {
      MaybeFunctionalWrappers.TryGet<string, string?> tryGetValue = 
          new Dictionary<string, string?>().TryGetValue;
      var wrapped = MaybeFunctionalWrappers.WrapNrt(tryGetValue);

      var result = wrapped("a");

      Assert.AreEqual(Maybe<string>.Nothing, result);
    }
  }
}
