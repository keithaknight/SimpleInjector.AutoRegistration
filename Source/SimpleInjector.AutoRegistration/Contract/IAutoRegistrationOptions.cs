using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleInjector.AutoRegistration.Contract
{
    /// <summary>
    /// Configuration options and extensibility point for configuring Auto-Registration of SimpleInjector.
    /// </summary>
    public interface IAutoRegistrationOptions
    {
        /// <summary>
        /// Controls which types will be auto-registered.  By default, all types in the application root
        /// namespace will be registered unless otherwise specified.  When using the built-in provider, you may
        /// specify type or assembly names to exclude from auto-registration.  Custom providers can be specified 
        /// to replace or extend this one.
        /// </summary>
        IAutoRegistrationEnabledProvider AutoRegistrationEnabledProvider { get; set; }

        /// <summary>
        /// Provides the services and concrete implementations to auto-register.  Custom providers
        /// can be specified to replace or extend this one.
        /// </summary>
        IImplementationProvider ImplementationProvider { get; set; }

        /// <summary>
        /// Resolves the lifestyle of types for auto-registration.  By default, all types will be transient unless 
        /// they have an ImmutableObjectAttribute that specifies that the type is immutable.  Custom resolvers can be specified
        /// to replace or extend this one.
        /// </summary>
        ILifestyleResolver LifestyleResolver { get; set; }

        /// <summary>
        /// Providers for discovering and registering dependencies of each type.  This collection can be replaced or extended with
        /// custom providers.
        /// </summary>
        IEnumerable<IDependencyRegistrationProvider> DependencyRegistrationProviders { get; set; }

        /// <summary>
        /// Gets or sets if decorators should be discovered and automatically registered.
        /// </summary>
        bool RegisterDecorators { get; set; }
    }
}
