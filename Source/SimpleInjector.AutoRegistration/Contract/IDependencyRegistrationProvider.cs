using System;

namespace SimpleInjector.AutoRegistration.Contract
{
    /// <summary>
    /// Provides discovery and registration of dependencies of auto-registration enabled
    /// types.
    /// </summary>
    public interface IDependencyRegistrationProvider
    {
        /// <summary>
        /// Discovers specific dependencies of the specified type and attempts to register them.
        /// </summary>
        /// <param name="concreteType">Concrete type for which the dependencies should be discovered
        /// and registered</param>
        /// <param name="container">SimpleInjector container in which the dependencies should be registered.</param>
        /// <param name="options">Options to apply to the discovery and registration process.</param>
        void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options);
    }
}
