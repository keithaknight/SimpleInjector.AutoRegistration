using System;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    /// <summary>
    /// Provides discovery and registration of the Lazy<> dependencies to auto-registration enabled
    /// types.
    /// </summary>
    [Export(typeof(IDependencyRegistrationProvider))]
    public class EnumerableDependencyRegistrationProvider : IDependencyRegistrationProvider
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public EnumerableDependencyRegistrationProvider()
        {
        }

        /// <summary>
        /// Discovers any IEnumerable<> dependencies of the specified type and attempts to register them.
        /// </summary>
        /// <param name="concreteType">Concrete type for which the dependencies should be discovered
        /// and registered</param>
        /// <param name="container">SimpleInjector container in which the dependencies should be registered.</param>
        /// <param name="options">Options to apply to the discovery and registration process.</param>
        public void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options)
        {
            foreach (var ctor in concreteType.GetConstructors().Where((x) => x.IsPublic))
            {
                foreach (var param in ctor.GetParameters().Where((x) => typeof(System.Collections.IEnumerable).IsAssignableFrom(x.ParameterType)))
                {
                    var genericArgType = param.ParameterType.GenericTypeArguments.FirstOrDefault();

                    if (genericArgType != null 
                     && genericArgType.IsInterface 
                     && options.AutoRegistrationEnabledProvider.IsAutoRegistrationEnabled(genericArgType))
                    {
                        container.RegisterAll(genericArgType, options.ImplementationProvider.GetConcreteImplementionsOf(genericArgType));
                    }
                }
            }
        }
    }
}
