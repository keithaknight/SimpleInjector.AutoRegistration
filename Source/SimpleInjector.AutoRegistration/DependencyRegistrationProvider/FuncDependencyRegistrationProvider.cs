using System;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    /// <summary>
    /// Provides discovery and registration of the Func dependencies to auto-registration enabled
    /// types.
    /// </summary>
    [Export(typeof(IDependencyRegistrationProvider))]
    public class FuncDependencyRegistrationProvider : IDependencyRegistrationProvider
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public FuncDependencyRegistrationProvider()
        {          
        }

        /// <summary>
        /// Discovers any Func dependencies of the specified type and attempts to register them.
        /// </summary>
        /// <param name="concreteType">Concrete type for which the dependencies should be discovered
        /// and registered</param>
        /// <param name="container">SimpleInjector container in which the dependencies should be registered.</param>
        /// <param name="options">Options to apply to the discovery and registration process.</param>
        public void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options)
        {
            foreach (var ctor in concreteType.GetConstructors().Where((x) => x.IsPublic))
            {
                foreach (var param in ctor.GetParameters().Where((x) => x.ParameterType.Name.StartsWith("Func`")))
                {
                    var funcParamType = param.ParameterType;
                    var genericArgType = funcParamType.GenericTypeArguments.FirstOrDefault();

                    if (genericArgType != null 
                     && genericArgType.IsInterface 
                     && options.AutoRegistrationEnabledProvider.IsAutoRegistrationEnabled(genericArgType))
                    {
                        //Since Funcs<> do not retain any state and the wrapped Func<> accounts for the
                        //lifestyle of the object, it can always be made singleton, while the wrapped
                        //type may be transient.
                        container.RegisterSingle(funcParamType, () =>
                        {
                            //Create a Func<> with the container in context to create an instance of the type
                            var method = typeof(ContextualFuncCreationHelper).GetMethod("GetFunc").MakeGenericMethod(genericArgType);
                            var funcInstance = new ContextualFuncCreationHelper(container);

                            return Delegate.CreateDelegate(funcParamType, funcInstance, method);
                        });
                    }
                }
            }
        }
    }
}
