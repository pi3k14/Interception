using System.Reflection;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception
{
    /// <summary>
    /// An <see cref="IInterceptor{TIntercepted}"/> implementation that invokes an instantiated class.
    /// Usefull as last interceptor in a chain of interceptors.
    /// </summary>
    /// <typeparam name="TClass">The type of the concrete object to invoke.</typeparam>
    public class ImplementationDispatcher<TClass> : IInterceptor<TClass> where TClass : class
    {
        readonly TClass _implementation;

        /// <summary>
        /// Creates an <see cref="ImplementationDispatcher{TClass}"/>.
        /// </summary>
        /// <param name="obj">Concrete object to invoke methods on.</param>
        public ImplementationDispatcher(TClass obj)
        {
            _implementation = obj;
        }

        /// <inheritdoc/>
        public object Intercept(MethodInfo targetMethod, object[] args)
        {
            return targetMethod.Invoke(_implementation, args);
        }
    }
}
