using System;
using System.Reflection;

using Kodefabrikken.Interception;
using Kodefabrikken.Proxy;

namespace Kodefabrikken.Decorator
{
    /// <summary>
    /// Base class for class decorators.
    /// </summary>
    /// <typeparam name="TIntercepted">Type of the decorated object.</typeparam>
    public abstract class Decorator<TIntercepted> : ChainedInterceptor<TIntercepted>, IDecorator<TIntercepted>
        where TIntercepted : class
    {
        /// <summary>
        /// Creates a new <see cref="Decorator{TIntercepted}"/>.
        /// </summary>
        /// <param name="targetObject">Interceptor that already wraps object to be decorated.
        /// First <see cref="IInterceptor{TIntercepted}"/> is created by proxy for intercepted object,
        /// which is an <see cref="ImplementationDispatcher{TClass}"/> if the class is wrapped by
        /// <see cref="Interception.InterceptedExtensions.AddChainedInterceptors{TInterceptor, TIntercepted}(TIntercepted)"/>.</param>
        public Decorator(IInterceptor<TIntercepted> targetObject) : base(targetObject)
        {
        }

        #region ChainedInterceptor

        /// <inheritdoc/>
        public override object Intercept(MethodInfo targetMethod, object[] args)
        {
            try
            {
                Before(targetMethod, args);

                object result = base.Intercept(targetMethod, args);

                After(targetMethod, args, ref result);

                return result;
            }
            catch (TargetInvocationException ex)
            {
                OnException(targetMethod, args, ex.InnerException);

                throw ex.InnerException;
            }
            finally
            {
                Finally(targetMethod, args);
            }
        }

        #endregion

        #region IDecorator

        /// <inheritdoc/>
        public virtual void Before(MethodInfo targetMethod, object[] args) { }

        /// <inheritdoc/>
        public virtual void After(MethodInfo targetMethod, object[] args, ref object result) { }

        /// <inheritdoc/>
        public virtual void OnException(MethodInfo targetMethod, object[] args, Exception ex) { }

        /// <inheritdoc/>
        public virtual void Finally(MethodInfo targetMethod, object[] args) { }

        #endregion
    }
}
