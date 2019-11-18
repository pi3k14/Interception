using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception.UnitTests
{
    public class ChainedInterceptorsTest
    {
        public interface IIntercepted
        {
            int Method();
        }

        public class FirstInterceptor : ChainedInterceptor<IIntercepted>
        {
            public FirstInterceptor(IInterceptor<IIntercepted> next) : base(next)
            {
            }

            public override object Intercept(MethodInfo targetMethod, object[] args)
            {
                return ((int)base.Intercept(targetMethod, args)) + 1;
            }
        }

        [TestClass]
        public class Intercept
        {
            [TestMethod]
            public void Method_implementation_is_called_on_intercept()
            {
                var intercepted = A.Fake<IIntercepted>();
                var sut = new ChainedInterceptors<IIntercepted>(intercepted);
                var arg1 = typeof(IIntercepted).GetMethod("Method");
                var arg2 = A.Dummy<object[]>();

                sut.Intercept(arg1, arg2);

                A.CallTo(() => intercepted.Method()).MustHaveHappenedOnceExactly();
            }
        }

        [TestClass]
        public class Add
        {
            [TestMethod]
            public void Interceptors_called_in_order()
            {
                var intercepted = A.Fake<IIntercepted>();
                A.CallTo(() => intercepted.Method()).Returns(1);
                var sut = new ChainedInterceptors<IIntercepted>(intercepted);
                sut.Add<FirstInterceptor>();
                var arg1 = typeof(IIntercepted).GetMethod("Method");
                var arg2 = A.Dummy<object[]>();

                // intercepter add one to method result, result should be two (one on fault)
                var result = sut.Intercept(arg1, arg2);

                Assert.AreEqual(2, result);
            }
        }
    }
}
