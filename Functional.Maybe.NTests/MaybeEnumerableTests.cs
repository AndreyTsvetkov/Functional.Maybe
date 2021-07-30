using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Functional.Maybe.Tests
{
  [TestFixture]
  public class MaybeEnumerableTests
  {
    [Test]
    public void WhereValueExist_Should_remove_Nothing_values()
    {
      var sequence = new[] { 1.ToMaybe(), Maybe<int>.Nothing, 2.ToMaybe() };
      int[] expected = { 1, 2 };

      var actual = sequence.WhereValueExist().ToArray();

      Assert.AreEqual(expected.Length, actual.Length);
      for (int i = 0; i < expected.Length; i++)
      {
        Assert.AreEqual(expected[i], actual[i]);
      }
    }

    [Test]
    public void SelectWhereValueExist_Should_work_with_null_returned_from_transformation()
    {
      var sequence = new[] { "a".ToMaybe(), "b".ToMaybe(), "c".ToMaybe() };
      var expected = new string?[] { null, null, null };

      string?[] actual = sequence.SelectWhereValueExist<string, string?>(s => null).ToArray();

      CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void Given_ThreeSome_UnionReturnsCollectionOfAll()
    {
      var one = 1.ToMaybe();
      var two = 2.ToMaybe();
      var three = 3.ToMaybe();

      var res = one.Union(two, three);
      Assert.AreEqual(3, res.Count());
      Assert.IsTrue(res.SequenceEqual(new[] { 1, 2, 3 }));
    }

    [Test]
    public void Given_EnumerableOfMaybes_SelectShouldWorkWithNullValues()
    {
      var enumerable = new[] { "a".ToMaybe(), "b".ToMaybe(), "c".ToMaybe() };

      var results = enumerable.Select<string, string>(s => null);

      CollectionAssert.AreEqual(new[]
      {
        Maybe<string>.Nothing,
        Maybe<string>.Nothing,
        Maybe<string>.Nothing,
      }, results);
    }

    [Test]
    public void Given_OneSome_UnionReturnsCollectionOfOne()
    {
      var one = 1.ToMaybe();
      var two = Maybe<int>.Nothing;

      var res = one.Union(two);
      Assert.AreEqual(1, res.Count());
      Assert.IsTrue(res.SequenceEqual(new[] { 1 }));
    }

    [Test]
    public void Given_CollectionAndSome_UnionReturnsCollectionPlusSome()
    {
      var one = new[] { 1, 3 };
      var two = 2.ToMaybe();

      var res = one.Union(two);
      Assert.AreEqual(3, res.Count());
      Assert.IsTrue(res.SequenceEqual(new[] { 1, 3, 2 }));
    }

    [Test]
    public void FirstMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
    {
      var maybe = Enumerable.Empty<object>().FirstMaybe();

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void FirstMaybeNrt_WhenCalledOnEmptyEnumerable_ReturnsNothing()
    {
      var maybe = Enumerable.Empty<object?>().FirstMaybeNrt();

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void FirstMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
    {
      var collection = new[] { 1, 2 };
      var itemToSearch = 3;

      var maybe = collection.FirstMaybe(i => i == itemToSearch);

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void FirstMaybe_WhenCalledOnEnumerableWithMatches_ReturnsFirstMatchingElement()
    {
      var expectedItem = Tuple.Create(2);
      var collection = new[]
      {
        Tuple.Create(1),
        expectedItem, // first matching element
        Tuple.Create(2), // last matching element
        Tuple.Create(3)
      };

      var maybe = collection.FirstMaybe(i => i.Item1 == 2);

      Assert.IsTrue(maybe.IsSomething());
      // use AreSame to compare expected value with actual one by reference
      Assert.AreSame(expectedItem, maybe.Value);
    }

    [Test]
    public void SingleMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
    {
      var maybe = Enumerable.Empty<object>().SingleMaybeNrt();

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void SingleMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
    {
      var collection = new[] { 1, 2 };
      var itemToSearch = 3;

      var maybe = collection.SingleMaybe(i => i == itemToSearch);

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithMultipleMatches_ReturnsNothing()
    {
      var collection = new[] { 1, 1, 2 };
      var itemToSearch = 1;

      var maybe = collection.SingleMaybe(i => i == itemToSearch);

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithSingleMatch_ReturnsSingleMatchingElement()
    {
      var collection = new[] { 1, 2, 3 };
      var itemToSearch = 2;

      var maybe = collection.SingleMaybe(i => i == itemToSearch);

      Assert.IsTrue(maybe.IsSomething());
      Assert.AreEqual(itemToSearch, maybe.Value);
    }

    [Test]
    public void LastMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
    {
      var maybe = Enumerable.Empty<object>().LastMaybe();

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void LastMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
    {
      var collection = new[] { 1, 2 };
      var itemToSearch = 3;

      var maybe = collection.LastMaybe(i => i == itemToSearch);

      Assert.IsTrue(maybe.IsNothing());
    }

    [Test]
    public void LastMaybe_WhenCalledOnEnumerableWithMatches_ReturnsLastMatchingElement()
    {
      var expectedItem = Tuple.Create(2);
      var collection = new[]
      {
        Tuple.Create(1),
        Tuple.Create(2), // first matching element
        expectedItem, // last matching element
        Tuple.Create(3)
      };

      var maybe = collection.LastMaybe(i => i.Item1 == 2);

      Assert.IsTrue(maybe.IsSomething());
      // use AreSame to compare expected value with actual one by reference
      Assert.AreSame(expectedItem, maybe.Value);
    }
  }
}