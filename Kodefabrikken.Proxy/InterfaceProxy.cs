using System;
using System.Reflection;

namespace Kodefabrikken.Proxy
{
    /// <summary>
    /// A <see cref="DispatchProxy"/> that lets an external class intercept the method calls.
    /// </summary>
    /// <typeparam name="TInterface">The interface this creates a proxy for.</typeparam>
    public class InterfaceProxy<TInterface> : DispatchProxy where TInterface : class
    {
        /// <summary>
        /// The <see cref="IInterceptor{TIntercepted}"/> used when intercepting method calls.
        /// </summary>
        public IInterceptor<TInterface> Interceptor { get; private set; }

        /// <summary>
        /// Creates the proxy object.
        /// </summary>
        /// <param name="interceptor">The <see cref="IInterceptor"/> to use for call handling of
        /// interface <typeparamref name="TInterface"/>.</param>
        /// <returns>The proxy for interface <typeparamref name="TInterface"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="interceptor"/> is null.</exception>
        public static TInterface Create(IInterceptor<TInterface> interceptor)
        {
            if (interceptor == null) // don't simplify this, we wan't exception before creating anything
            {
                throw new ArgumentNullException(nameof(interceptor));
            }

            // The created type is derived from InterfaceProxy<TInterface>
            var obj = Create<TInterface, InterfaceProxy<TInterface>>();

            // This should never be null
            (obj as InterfaceProxy<TInterface>).Interceptor = interceptor;

            return obj;
        }

        /// <summary>
        /// Call the interceptor to handle the call.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        /// <returns>The target method result (from the dispatcher).</returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            return Interceptor.Intercept(targetMethod, args);
        }
    }
}
