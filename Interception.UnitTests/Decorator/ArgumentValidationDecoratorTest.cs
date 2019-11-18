using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

using Kodefabrikken.Interception;


namespace Kodefabrikken.Decorator.UnitTests
{
    public class ArgumentValidationDecoratorTest
    {
        public interface IInterface
        {
            void Method(object arg);
            void Method2(Nullable<int> arg);
            void Method3([Nullable] object arg);
        }

        [TestClass]
        public class Before
        {
            [TestMethod]
            public void Exception_thrown_when_argument_is_null()
            {
                var intercepted = A.Dummy<IInterface>();
                var sut = new ArgumentValidationDecorator<IInterface>(new ImplementationDispatcher<IInterface>(intercepted));

                Assert.ThrowsException<ArgumentNullException>(() => sut.Intercept(typeof(IInterface).GetMethod("Method"), new object[1] { null }));
            }

            [TestMethod]
            public void Null_allowed_when_argument_is_nullable_type()
            {
                var intercepted = A.Dummy<IInterface>();
                var sut = new ArgumentValidationDecorator<IInterface>(new ImplementationDispatcher<IInterface>(intercepted));

                sut.Intercept(typeof(IInterface).GetMethod("Method2"), new object[1] { null });
            }

            [TestMethod]
            public void Null_allowed_when_argument_has_nullable_attribute()
            {
                var intercepted = A.Dummy<IInterface>();
                var sut = new ArgumentValidationDecorator<IInterface>(new ImplementationDispatcher<IInterface>(intercepted));

                sut.Intercept(typeof(IInterface).GetMethod("Method3"), new object[1] { null });
            }
        }
    }
}
