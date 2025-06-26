using System;
using Functional.Either;
using NUnit.Framework;

namespace Functional.Maybe.NTests;

[TestFixture]
public class EitherTests
{
    private readonly Either<int, string> _eitherResult = EitherLeftValue.ToResult<int, string>();
    private readonly Either<int, string> _eitherError = EitherRightValue.ToError<int, string>();
    private const int EitherLeftValue = 5;
    private const string EitherRightValue = "Five";

#pragma warning disable 219

    [Test]
    public void NullCheckingTests()
    {
        Action<int> nullActionInt = null;

        void MockActionInt(int x)
        {
            var y = 5;
        }

        Action<string> nullActionString = null;

        void MockActionString(string x)
        {
            var y = 5;
        }

        Action nullAction = null;

        void MockAction()
        {
            var a = 1;
        }

        // ReSharper disable ExpressionIsAlwaysNull
        AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(nullAction, MockAction));
        AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(MockAction, nullAction));
        _eitherResult.Match(MockAction, MockAction);

        AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullAction, MockAction));
        AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(MockAction, nullAction));
        _eitherError.Match(MockAction, MockAction);

        AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullActionInt, MockActionString));
        AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(MockActionInt, nullActionString));
        _eitherResult.Match(MockActionInt, MockActionString);
    }

    [Test]
    public void MatchActionTests()
    {
        var bool1 = false;
        var bool2 = false;
        var testInt = 0;
        var testString = null as string;

        Action resetTestValues = () =>
        {
            bool1 = false;
            bool2 = false;
            testInt = 0;
            testString = null;
        };

        Action setBool1Action = () => bool1 = true;
        Action setBool2Action = () => bool2 = true;

        Action<int> setTestInt = value => testInt = value;
        Action<string> setTestString = value => testString = value;

        _eitherResult.Match(setBool1Action, setBool2Action);
        Assert.IsTrue(bool1);
        Assert.IsFalse(bool2);

        resetTestValues();
        _eitherError.Match(setBool1Action, setBool2Action);
        Assert.IsFalse(bool1);
        Assert.IsTrue(bool2);

        resetTestValues();
        _eitherResult.Match(setTestInt, setTestString);
        Assert.AreEqual(EitherLeftValue, testInt);
        Assert.AreEqual(null, testString);

        resetTestValues();
        _eitherError.Match(setTestInt, setTestString);
        Assert.AreEqual(0, testInt);
        Assert.AreEqual(EitherRightValue, testString);

        resetTestValues();
    }

    [Test]
    public void MatchFunctionTests()
    {
        var testInt = 0;
        var testString = null as string;

        Action resetTestValues = () =>
        {
            testInt = 0;
            testString = null;
        };

        Func<int, bool> funcTLT = x =>
        {
            testInt = x;
            return true;
        };

        Func<string, bool> funcTRT = x =>
        {
            testString = x;
            return false;
        };

        Assert.IsTrue(_eitherResult.Match(funcTLT, funcTRT));
        Assert.AreEqual(EitherLeftValue, testInt);
        Assert.AreEqual(null, testString);

        resetTestValues();
        Assert.IsFalse(_eitherError.Match(funcTLT, funcTRT));
        Assert.AreEqual(EitherRightValue, testString);
        Assert.AreEqual(0, testInt);

        resetTestValues();
        Assert.IsTrue(_eitherResult.Match(() => true, () => false));
        Assert.IsFalse(_eitherError.Match(() => true, () => false));
    }

    [Test]
    public void OrDefaultFunctionsTests()
    {
        Assert.AreEqual(EitherLeftValue, _eitherResult.ResultOrDefault());
        Assert.AreEqual(EitherRightValue, _eitherError.ErrorOrDefault());

        Assert.AreEqual(0, _eitherError.ResultOrDefault());
        Assert.AreEqual(default, _eitherResult.ErrorOrDefault());

        Assert.AreEqual(29, _eitherError.ResultOrDefault(29));
        Assert.AreEqual("Twenty nine", _eitherResult.ErrorOrDefault("Twenty nine"));
    }

    [Test]
    public void SameTResultTErrorTests()
    {
        var eitherResult = Either<string, string>.Result("Left defined");
        var eitherError = Either<string, string>.Error("Right defined");

        Assert.AreEqual("Left defined", eitherResult.ResultOrDefault());
        Assert.AreEqual("Right defined", eitherError.ErrorOrDefault());

        Assert.AreEqual(null, eitherError.ResultOrDefault());
        Assert.AreEqual(null, eitherResult.ErrorOrDefault());
    }

    [Test]
    public void ExtensionMethodTests()
    {
        var eitherResult = 29.ToResult<int, string>();
        var eitherError = "Twenty nine".ToError<int, string>();

        Assert.AreEqual(29, eitherResult.ResultOrDefault());
        Assert.AreEqual(null, eitherResult.ErrorOrDefault());

        Assert.AreEqual("Twenty nine", eitherError.ErrorOrDefault());
        Assert.AreEqual(0, eitherError.ResultOrDefault());
    }
}


internal static class AssertExtension
{
    public static void Throws<T>(Action action) where T : Exception
    {
        var exceptionThrown = false;
        try
        {
            action.Invoke();
        }
        catch (T)
        {
            exceptionThrown = true;
        }

        if (!exceptionThrown)
        {
            throw new Exception(
                $"An exception of type {typeof(T)} was expected, but not thrown"
            );
        }
    }
}