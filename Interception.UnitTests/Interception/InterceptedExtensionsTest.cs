using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception.UnitTests
{
    public class InterceptedExtensionsTest
    {
        public interface IIntercepted
        {
            void Method();
        }

        public class Interceptor : ChainedInterceptor<IIntercepted>
        {
            public Interceptor(IInterceptor<IIntercepted> next) : base(next)
            {
            }
        }

        [TestClass]
        public class AddChainedInterceptor
        {
            [TestMethod]
            public void Proxy_created_when_intercepted_object_not_proxied()
            {
                var intercepted = A.Fake<IIntercepted>();

                var sut = intercepted.AddChainedInterceptors<Interceptor, IIntercepted>();

                Assert.IsInstanceOfType(sut, typeof(InterfaceProxy<IIntercepted>));
            }

            [TestMethod]
            public void New_proxy_not_created_when_intercepted_object_allready_proxied()
            {
                var intercepted = A.Fake<IIntercepted>();
                var tmp = intercepted.AddChainedInterceptors<Interceptor, IIntercepted>();

                var sut = tmp.AddChainedInterceptors<Interceptor, IIntercepted>();

                Assert.AreSame(sut, tmp);
            }
        }
    }
}

