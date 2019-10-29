using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

namespace Kodefabrikken.Decorator.UnitTests
{
    public class InterceptedExtensionsTest
    {
        public interface IDecorated
        {
            void Method(object arg);
        }

        [TestClass]
        public class AddArgumentValidationDecorator
        {
            [TestMethod]
            public void Expected_decorator_is_added()
            {
                var actualObject = A.Dummy<IDecorated>();

                var sut = actualObject.AddArgumentValidationDecorator<IDecorated>();

                Assert.ThrowsException<ArgumentNullException>(() => sut.Method(null));
                sut.Method(A.Dummy<object>());
            }
        }
    }
}
