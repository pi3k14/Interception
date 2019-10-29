using System;
using System.Reflection;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Decorator
{
    /// <summary>
    /// Interface for decoration of methods (via interception).
    /// </summary>
    /// <typeparam name="TIntercepted">Type of intercepted interface.</typeparam>
    public interface IDecorator<TIntercepted> : IInterceptor<TIntercepted> 
        where TIntercepted : class
    {
        /// <summary>
        /// Is run before method is executed.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        void Before(MethodInfo targetMethod, object[] args);

        /// <summary>
        /// Is run after method is executed, unless there is an exception.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        /// <param name="result">The method result.</param>
        void After(MethodInfo targetMethod, object[] args, ref object result);

        /// <summary>
        /// Is run if an exception occurs in the executed method.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        /// <param name="ex">The exception that was thrown.</param>
        void OnException(MethodInfo targetMethod, object[] args, Exception ex);

        /// <summary>
        /// Is executed last, even after an exception.
        /// </summary>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="args">The method arguments.</param>
        void Finally(MethodInfo targetMethod, object[] args);
    }
}
