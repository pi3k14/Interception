using System.Reflection;

namespace Kodefabrikken.Proxy
{
    /// <summary>
    /// Definition of a method interceptor.
    /// </summary>
    /// <typeparam name="TIntercepted">Intercepted interface.</typeparam>
    public interface IInterceptor<TIntercepted> where TIntercepted : class
    {
        /// <summary>
        /// Intercepting given target method.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        /// <returns>The method result.</returns>
        object Intercept(MethodInfo targetMethod, object[] args);
    }
}