using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using Kodefabrikken.Proxy;

[assembly: InternalsVisibleTo("Kodefabrikken.Interception.UnitTests")]

namespace Kodefabrikken.Interception
{
    /// <summary>
    /// An <see cref="IInterceptor{TIntercepted}"/> that administrate the chaining of several
    /// interceptors for an <see cref="InterfaceProxy{TInterface}"/>.
    /// This is always the first interceptor in the chain.
    /// TODO : Should this also be the end of the chain, dispatching to implementation?
    /// Used internally by <see cref="InterceptedExtensions.AddChainedInterceptors{TInterceptor, TIntercepted}(TIntercepted)"/>.
    /// </summary>
    /// <typeparam name="TIntercepted">Type of intercepted interface.</typeparam>
    /// <remarks>Not for public use.</remarks>
    public class ChainedInterceptors<TIntercepted> : IInterceptor<TIntercepted>
        where TIntercepted : class
    {
        IInterceptor<TIntercepted> FirstInterceptor { get; set; }

        internal ChainedInterceptors(IInterceptor<TIntercepted> interceptor)
        {
            FirstInterceptor = interceptor;
        }

        internal IInterceptor<TIntercepted> Add<TInterceptor>()
            where TInterceptor : ChainedInterceptor<TIntercepted>
        {
            FirstInterceptor = (TInterceptor)Activator.CreateInstance(typeof(TInterceptor), FirstInterceptor);

            return FirstInterceptor;
        }

        /// <inheritdoc/>
        public object Intercept(MethodInfo targetMethod, object[] args)
        {
            return FirstInterceptor.Intercept(targetMethod, args);
        }
    }
}
