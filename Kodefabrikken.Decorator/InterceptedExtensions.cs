using System;

using Kodefabrikken.Interception;

namespace Kodefabrikken.Decorator
{
    /// <summary>
    /// Extension method(s) for intercepted objects.
    /// </summary>
    public static partial class InterceptedExtensions
    {
        /// <summary>
        /// Add a new <see cref="ArgumentValidationDecorator{TIntercepted}"/> to the <typeparamref name="TIntercepted"/> object.
        /// Decorator is placed in front of existing decorators, chain is ended with call to actual object.
        /// </summary>
        /// <typeparam name="TIntercepted">Type of the decorated object.</typeparam>
        /// <param name="obj">The decorated object.</param>
        /// <returns>The new decorator, that wraps the original object.</returns>
        public static TIntercepted AddArgumentValidationDecorator<TIntercepted>(this TIntercepted obj)
            where TIntercepted : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return obj.AddChainedInterceptors<ArgumentValidationDecorator<TIntercepted>, TIntercepted>();
        }
    }
}
