using System;
using System.Linq;
using System.Reflection;

using Kodefabrikken.Proxy;

namespace Kodefabrikken.Decorator
{
    /// <summary>
    /// Parameter attribute that allows null value for parameter./>
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NullableAttribute : Attribute
    {
    }

    /// <summary>
    /// Decorator that adds null validation on parameters for all methods.
    /// Null value will only be allowed for parameters attributed with <see cref="NullableAttribute"/>
    /// or of a <see cref="Nullable{T}"/> type.
    /// </summary>
    /// <typeparam name="TIntercepted">Type of the object decorated.</typeparam>
    public class ArgumentValidationDecorator<TIntercepted> : Decorator<TIntercepted>
        where TIntercepted : class
    {
        /// <summary>
        /// Creates a new <see cref="ArgumentValidationDecorator{TIntercepted}"/>.
        /// </summary>
        /// <param name="target">Interceptor that already wraps object to be decorated.
        /// First <see cref="IInterceptor{TIntercepted}"/> is created by proxy for intercepted object,
        /// which is an <see cref="ImplementationDispatcher{TClass}"/> if the class is wrapped by
        /// <see cref="Interception.InterceptedExtensions.AddChainedInterceptors{TInterceptor, TIntercepted}(TIntercepted)"/>.</param>
        public ArgumentValidationDecorator(IInterceptor<TIntercepted> target) : base(target)
        {
        }

        /// <inheritdoc/>
        public override void Before(MethodInfo targetMethod, object[] args)
        {
            foreach (var parameterInfo in targetMethod.GetParameters())
            {
                if (!parameterInfo.ParameterType.IsValueType && // not value type
                    !(parameterInfo.ParameterType.IsGenericType && parameterInfo.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>)) && // not Nullable<T>
                    !parameterInfo.CustomAttributes.Any(p => p.AttributeType == typeof(NullableAttribute))) // no Nullable attribute
                {
                    if (args[parameterInfo.Position] == null)
                    {
                        throw new ArgumentNullException(parameterInfo.Name);
                    }
                }
            }
        }
    }
}
