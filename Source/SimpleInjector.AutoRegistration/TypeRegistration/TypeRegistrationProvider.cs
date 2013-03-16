using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector.Extensions;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration.TypeRegistration
{
    public class TypeRegistrationProvider
    {
        private readonly Container container;
        private readonly IAutoRegistrationOptions options;

        public TypeRegistrationProvider(Container container, IAutoRegistrationOptions options)
        {
            this.container = container;
            this.options = options;

            this.container.Options.AllowOverridingRegistrations = true;
        }

        public void RegisterTypes()
        {
            foreach (var concreteType in options.ImplementationProvider.GetAllConcreteTypes())
            {
                foreach (var registrationProvider in options.DependencyRegistrationProviders)
                {
                    registrationProvider.RegisterDependencies(concreteType, this.container, this.options);
                }

                foreach (var serviceType in concreteType.GetInterfaces().Where((x) => options.AutoRegistrationEnabledProvider.IsAutoRegistrationEnabled(x)))
                {
                    RegisterType(concreteType, serviceType);
                }
            }
        }

        private void RegisterType(Type concreteType, Type serviceType)
        {
            bool isOpenGeneric = IsOpenGeneric(concreteType);
            if (isOpenGeneric)
            {
                var genericServiceType = options.ImplementationProvider.GetNonRuntimeGenericInterface(serviceType);
                isOpenGeneric = true;

                if (genericServiceType == null)
                {
                    throw new NotSupportedException(string.Format("Not sure how to register type: {0}", concreteType.FullName));
                }
                serviceType = genericServiceType;
            }

            Lifestyle lifeStyle = options.LifestyleResolver.GetLifestyle(concreteType);

            if (IsDecoratorType(concreteType))
            {
                if (options.RegisterDecorators)
                {
                    this.container.RegisterDecorator(serviceType, concreteType, lifeStyle);
                }
            }
            else
            {
                if (isOpenGeneric)
                {
                    this.container.RegisterOpenGeneric(serviceType, concreteType, lifeStyle);
                }
                else
                {
                    this.container.Register(serviceType, concreteType, lifeStyle);
                }
            }
        }

        private bool IsOpenGeneric(Type type)
        {
            return type.IsGenericType && type.ContainsGenericParameters;
        }

        private bool IsDecoratorType(Type type)
        {
            Lazy<HashSet<Type>> servicesExposed = new Lazy<HashSet<Type>>(() =>
            {
                HashSet<Type> set = new HashSet<Type>();
                foreach (var serviceType in type.GetInterfaces())
                {
                    set.Add(serviceType);
                }
                return set;
            });

            bool isDecorator = false;

            foreach (var ctor in type.GetConstructors().Where((x) => x.IsPublic))
            {
                isDecorator = ctor.GetParameters().Any((x) => servicesExposed.Value.Contains(x.ParameterType));
                if (isDecorator) break;
            }

            return isDecorator;
        }
    }
}
