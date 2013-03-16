using System;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    [Export(typeof(IDependencyRegistrationProvider))]
    public class EnumerableDependencyRegistrationProvider : IDependencyRegistrationProvider
    {
        public EnumerableDependencyRegistrationProvider()
        {
        }

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
