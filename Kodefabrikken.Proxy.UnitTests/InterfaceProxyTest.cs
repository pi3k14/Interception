using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

namespace Kodefabrikken.Proxy.UnitTests
{
    public class InterfaceProxyTest
    {
        public interface IIntercepted
        {
            void Method();
        }

        [TestClass]
        public class Interceptor
        {
            [TestMethod]
            public void Interceptor_is_set_when_creating_instance()
            {
                var interceptor = A.Dummy<IInterceptor<IIntercepted>>();

                var sut = InterfaceProxy<IIntercepted>.Create(interceptor) as InterfaceProxy<IIntercepted>;

                Assert.IsNotNull(sut);
                Assert.IsInstanceOfType(sut.Interceptor, interceptor.GetType());
            }
        }

        [TestClass]
        public class Create
        {
            [TestMethod]
            public void Interface_proxy_implementing_interface_is_created()
            {
                var interceptor = A.Dummy<IInterceptor<IIntercepted>>();

                IIntercepted sut = InterfaceProxy<IIntercepted>.Create(interceptor);

                Assert.IsNotNull(sut);
                Assert.IsInstanceOfType(sut, typeof(InterfaceProxy<IIntercepted>));
            }
        }

        [TestClass]
        public class Invoke
        {
            [TestMethod]
            public void Interceptor_is_called_when_using_proxy()
            {
                var interceptor = A.Dummy<IInterceptor<IIntercepted>>();
                var sut = InterfaceProxy<IIntercepted>.Create(interceptor);

                sut.Method();

                A.CallTo(() => interceptor.Intercept(A<System.Reflection.MethodInfo>.That.Matches(p => p.Name == "Method" && p.ReturnType == typeof(void)),
                                                     A<object[]>.That.IsEmpty())).MustHaveHappenedOnceExactly();
            }
        }
    }
}
