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
            void Method();
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
            public void Next_interceptor_in_chain_is_called_on_intercept()
            {
                var interceptor = A.Fake<IInterceptor<IIntercepted>>();
                var sut = new ChainedInterceptors<IIntercepted>(interceptor);
                var arg1 = A.Dummy<MethodInfo>();
                var arg2 = A.Dummy<object[]>();

                sut.Intercept(arg1, arg2);

                A.CallTo(() => interceptor.Intercept(arg1, arg2)).MustHaveHappenedOnceExactly();
            }
        }

        [TestClass]
        public class Add
        {
            [TestMethod]
            public void Interceptors_called_in_order()
            {
                var interceptor = A.Fake<IInterceptor<IIntercepted>>();
                var arg1 = A.Dummy<MethodInfo>();
                var arg2 = A.Dummy<object[]>();
                A.CallTo(() => interceptor.Intercept(arg1, arg2)).Returns(1);
                var sut = new ChainedInterceptors<IIntercepted>(interceptor);
                sut.Add<FirstInterceptor>();

                // outer intercepter add one to inner, result should be two (one on fault)
                var result = sut.Intercept(arg1, arg2);

                Assert.AreEqual(2, result);
            }
        }
    }
}
