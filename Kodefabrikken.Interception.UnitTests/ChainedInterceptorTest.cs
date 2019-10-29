using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception.UnitTests
{
    public class ChainedInterceptorTest
    {
        public interface IIntercepted
        {
            void Method();
        }

        [TestClass]
        public class Intercept
        {
            [TestMethod]
            public void Next_interceptor_is_called_during_interception()
            {
                var nextInterceptor = A.Fake<IInterceptor<IIntercepted>>();
                // sut faked because it is an abstract class
                var sut = A.Fake<ChainedInterceptor<IIntercepted>>(x => x.WithArgumentsForConstructor((new object[] { nextInterceptor })));
                A.CallTo(() => sut.Intercept(A<MethodInfo>.Ignored, A<object[]>.Ignored)).CallsBaseMethod();
                var arg1 = A.Dummy<MethodInfo>();
                var arg2 = A.Dummy<object[]>();

                sut.Intercept(arg1, arg2);

                A.CallTo(() => nextInterceptor.Intercept(arg1, arg2)).MustHaveHappenedOnceExactly();
            }
        }
    }
}
