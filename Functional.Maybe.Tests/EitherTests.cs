using Functional.Either;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Functional.Maybe.Tests
{
    [TestClass]
    public class EitherTests
    {
        private readonly Either<int, string> _eitherResult;
        private readonly Either<int, string> _eitherError;
        private const int _eitherLeftValue = 5;
        private const string _eitherRightValue = "Five";

        public EitherTests()
        {
            _eitherResult = _eitherLeftValue.ToResult<int, string>();
            _eitherError = _eitherRightValue.ToError<int, string>();
        }

        [TestMethod]
        public void NullCheckingTests()
        {
            Action<int> nullActionInt = null;
            Action<int> mockActionInt = x => { var y = 5; };

            Action<string> nullActionString = null;
            Action<string> mockActionString = x => { var y = 5; };

            Action nullAction = null;
            Action mockAction = () => { var a = 1; };

            AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(nullAction, mockAction));
            AssertExtension.Throws<ArgumentNullException>(() => _eitherResult.Match(mockAction, nullAction));
            _eitherResult.Match(mockAction, mockAction);

            AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullAction, mockAction));
            AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(mockAction, nullAction));
            _eitherError.Match(mockAction, mockAction);

            AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(nullActionInt, mockActionString));
            AssertExtension.Throws<ArgumentNullException>(() => _eitherError.Match(mockActionInt, nullActionString));
            _eitherResult.Match(mockActionInt, mockActionString);
        }

        [TestMethod]
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
            Assert.AreEqual(_eitherLeftValue, testInt);
            Assert.AreEqual(null, testString);

            resetTestValues();
            _eitherError.Match(setTestInt, setTestString);
            Assert.AreEqual(0, testInt);
            Assert.AreEqual(_eitherRightValue, testString);

            resetTestValues();
        }

        [TestMethod]
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
            Assert.AreEqual(_eitherLeftValue, testInt);
            Assert.AreEqual(null, testString);

            resetTestValues();
            Assert.IsFalse(_eitherError.Match(funcTLT, funcTRT));
            Assert.AreEqual(_eitherRightValue, testString);
            Assert.AreEqual(0, testInt);

            resetTestValues();
            Assert.IsTrue(_eitherResult.Match(() => true, () => false));
            Assert.IsFalse(_eitherError.Match(() => true, () => false));
        }

        [TestMethod]
        public void OrDefaultFunctionsTests()
        {
            Assert.AreEqual(_eitherLeftValue, _eitherResult.ResultOrDefault());
            Assert.AreEqual(_eitherRightValue, _eitherError.ErrorOrDefault());

            Assert.AreEqual(default, _eitherError.ResultOrDefault());
            Assert.AreEqual(default, _eitherResult.ErrorOrDefault());

            Assert.AreEqual(29, _eitherError.ResultOrDefault(29));
            Assert.AreEqual("Twenty nine", _eitherResult.ErrorOrDefault("Twenty nine"));
        }

        [TestMethod]
        public void SameTResultTErrorTests()
        {
            var eitherResult = Either<string, string>.Result("Left defined");
            var eitherError = Either<string, string>.Error("Right defined");

            Assert.AreEqual("Left defined", eitherResult.ResultOrDefault());
            Assert.AreEqual("Right defined", eitherError.ErrorOrDefault());

            Assert.AreEqual(null, eitherError.ResultOrDefault());
            Assert.AreEqual(null, eitherResult.ErrorOrDefault());
        }

        [TestMethod]
        public void ExtensionMethodTests()
        {
            var eitherResult = 29.ToResult<int, string>();
            var eitherError = "Twenty nine".ToError<int, string>();

            Assert.AreEqual(29, eitherResult.ResultOrDefault());
            Assert.AreEqual(null, eitherResult.ErrorOrDefault());
            Assert.IsTrue(eitherResult.Success);

            Assert.AreEqual("Twenty nine", eitherError.ErrorOrDefault());
            Assert.AreEqual(default, eitherError.ResultOrDefault());
            Assert.IsFalse(eitherError.Success);
        }
    }


    class AssertExtension
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
                throw new AssertFailedException(
                    String.Format("An exception of type {0} was expected, but not thrown", typeof(T))
                    );
            }
        }
    }
}
