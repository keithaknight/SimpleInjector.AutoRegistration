using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using SimpleInjector.Extensions;

namespace SimpleInjector.AutoRegistration.TypeRegistration
{
    /// <summary>
    /// Providers all registration logic for types that have auto-registration enabled.
    /// </summary>
    public class TypeRegistrationProvider
    {
        private readonly Container container;
        private readonly IAutoRegistrationOptions options;

        /// <summary>
        /// Constructor.  Creates a new TypeRegistrationProvider instance.
        /// </summary>
        /// <param name="container">SimpleInjector Container instance</param>
        /// <param name="options">Registration options to use to auto-register types.</param>
        public TypeRegistrationProvider(Container container, IAutoRegistrationOptions options)
        {
            this.container = container;
            this.options = options;

            this.container.Options.AllowOverridingRegistrations = true;
        }

        /// <summary>
        /// Discovers and registers all enabled types.
        /// </summary>
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

        /// <summary>
        /// Registers the specified concrete implementation for the specified service.
        /// </summary>
        /// <param name="concreteType">Concrete implementation to register</param>
        /// <param name="serviceType">Service fulfilled by the concrete implementation.</param>
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

        /// <summary>
        /// Determines if the specified type is an open generic.
        /// </summary>
        /// <param name="type">Type to analyze.</param>
        /// <returns>A value indicating if the specified type is an open generic.</returns>
        private bool IsOpenGeneric(Type type)
        {
            return type.IsGenericType && type.ContainsGenericParameters;
        }

        /// <summary>
        /// Determines if the specified type is a decorator of a service.
        /// </summary>
        /// <param name="type">Type to analyze.</param>
        /// <returns>A value indicating if the specified type is a decorator of a service.</returns>
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
