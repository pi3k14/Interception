using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakeItEasy;

namespace Kodefabrikken.Interception.UnitTests
{
    public class ImplementationDispatcherTest
    {
        public interface IIntercepted
        {
            void Method();
        }

        [TestClass]
        public class Intercept
        {
            [TestMethod]
            public void Implementation_is_called_during_interception()
            {
                var implementation = A.Fake<IIntercepted>();
                var sut = new ImplementationDispatcher<IIntercepted>(implementation);

                sut.Intercept(typeof(IIntercepted).GetMethod("Method"), new object[0]);

                A.CallTo(() => implementation.Method()).MustHaveHappenedOnceExactly();
            }
        }
    }
}
