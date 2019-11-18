using System.Reflection;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception
{
    /// <summary>
    /// Base class for an <see cref="IInterceptor{TIntercepted}"/> that chaines to another.
    /// </summary>
    /// <typeparam name="TIntercepted">Type of intercepted interface.</typeparam>
    public abstract class ChainedInterceptor<TIntercepted> : IInterceptor<TIntercepted> 
        where TIntercepted : class
    {
        internal IInterceptor<TIntercepted> _next;

        /// <summary>
        /// Creates a <see cref="ChainedInterceptor"/>.
        /// </summary>
        /// <param name="next">Next interceptor to call.</param>
        public ChainedInterceptor(IInterceptor<TIntercepted> next)
        {
            _next = next;
        }

        /// <inheritdoc/>
        public virtual object Intercept(MethodInfo targetMethod, object[] args)
        {
            return _next.Intercept(targetMethod, args);
        }
    }
}
