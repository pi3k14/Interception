using System;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

using Kodefabrikken.Interception;
using Kodefabrikken.Proxy;

namespace Kodefabrikken.Decorator.UnitTests
{
    public class DecoratorTest
    {
        public interface IDecorated
        {
            void Method();
        }

        public class RiggedDecorator : Decorator<IDecorated>
        {
            readonly Action<MethodInfo, object[]> _before;
            readonly Action<MethodInfo, object[]> _after;
            readonly Action<MethodInfo, object[]> _finally;

            public RiggedDecorator(IInterceptor<IDecorated> next,
                                   Action<MethodInfo, object[]> before = null,
                                   Action<MethodInfo, object[]> after = null,
                                   Action<MethodInfo, object[]> @finally = null) : base(next)
            {
                _before = before;
                _after = after;
                _finally = @finally;
            }

            public override void Before(MethodInfo targetMethod, object[] args)
            {
                _before?.Invoke(targetMethod, args);
            }

            public override void After(MethodInfo targetMethod, object[] args, ref object result)
            {
                _after?.Invoke(targetMethod, args);
            }

            public override void Finally(MethodInfo targetMethod, object[] args)
            {
                _finally?.Invoke(targetMethod, args);
            }
        }

        [TestClass]
        public class Intercept
        {
            [TestMethod]
            public void Exception_in_method_bubbles_up()
            {
                var intercepted = A.Fake<IDecorated>();
                A.CallTo(() => intercepted.Method()).Throws<NotImplementedException>();
                // sut is faked because it is abstract
                var sut = A.Fake<Decorator<IDecorated>>(x => x.WithArgumentsForConstructor(new object[] { new ImplementationDispatcher<IDecorated>(intercepted) }));
                A.CallTo(() => sut.Intercept(A<MethodInfo>.Ignored, A<object[]>.Ignored)).CallsBaseMethod();

                Assert.ThrowsException<NotImplementedException>(() => sut.Intercept(typeof(IDecorated).GetMethod("Method"), new object[0]));
            }

            [TestMethod]
            public void Decorator_methods_called_in_order()
            {
                int step = 0;
                var intercepted = A.Fake<IDecorated>();
                A.CallTo(() => intercepted.Method()).Invokes(() => { if (step != 1) throw new Exception("Intercept"); ++step; });
                var decorator = new RiggedDecorator(new ImplementationDispatcher<IDecorated>(intercepted),
                                                  (m, a) => { if (step != 0) throw new Exception("Before"); ++step; },
                                                  (m, a) => { if (step != 2) throw new Exception("After"); ++step; },
                                                  (m, a) => { if (step != 3) throw new Exception("Finally"); ++step; });

                // throws on fault
                decorator.Intercept(typeof(IDecorated).GetMethod("Method"), new object[0]);
            }
        }
    }
}
