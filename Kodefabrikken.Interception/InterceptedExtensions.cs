using System;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Interception
{
    /// <summary>
    /// Extension method(s) for intercepted objects.
    /// </summary>
    public static class InterceptedExtensions
    {
        /// <summary>
        /// Add a new <see cref="ChainedInterceptor{TIntercepted}"/> to the <typeparamref name="TIntercepted"/> object.
        /// Interceptor is placed in front of existing interceptors, chain is ended with call to actual object.
        /// </summary>
        /// <typeparam name="TInterceptor">Type of interceptor, must derive from <see cref="ChainedInterceptor{TIntercepted}"/>.</typeparam>
        /// <typeparam name="TIntercepted">Type of intercepted object.</typeparam>
        /// <param name="obj">Intercepted object (or proxy).</param>
        /// <returns>Intercepted object (its proxy).</returns>
        /// <exception cref="ArgumentException">Existing proxy wasn't created with a <see cref="ChainedInterceptors{TIntercepted}"/>.
        /// ie. <see cref="AddChainedInterceptors{TInterceptor, TIntercepted}(TIntercepted)"/> wasn't used for creating proxy.</exception>
        public static TIntercepted AddChainedInterceptors<TInterceptor, TIntercepted>(this TIntercepted obj)
            where TInterceptor : ChainedInterceptor<TIntercepted>
            where TIntercepted : class
        {
            ChainedInterceptors<TIntercepted> proxysInterceptor;

            var interfaceProxy = obj as InterfaceProxy<TIntercepted>;
            if (interfaceProxy == null) // if not already proxied, create a proxy
            {
                // TODO : Keep a collection of intercepted objects, so we can find the proxy even if the
                //        user send the proxied object on each call?
                proxysInterceptor = new ChainedInterceptors<TIntercepted>(obj);
                obj = InterfaceProxy<TIntercepted>.Create(proxysInterceptor);
            }
            else
            {
                proxysInterceptor = interfaceProxy.Interceptor as ChainedInterceptors<TIntercepted>;
                if (proxysInterceptor == null) // proxy was created without ChainedInterceptors, can't add
                {
                    // TODO : Create new chain with this proxy as last element?
                    throw new ArgumentException($"{nameof(obj)}: proxy doesn't contain a chainable interceptor");
                }
            }

            proxysInterceptor.Add<TInterceptor>();

            return obj;
        }
    }
}
