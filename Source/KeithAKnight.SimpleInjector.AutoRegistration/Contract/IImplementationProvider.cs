using System;
using System.Collections.Generic;
using System.Linq;

namespace KeithAKnight.SimpleInjector.AutoRegistration.Contract
{
    /// <summary>
    /// Discovers the appropriate concrete implementations and services to be auto-registered.
    /// </summary>
    public interface IImplementationProvider
    {
        /// <summary>
        /// Gets all concrete types that have auto-registration enabled.
        /// </summary>
        /// <returns>All concrete types that have auto-registration enabled.</returns>
        IEnumerable<Type> GetAllConcreteTypes();

        /// <summary>
        /// Gets the concrete implementations of the specified service type.
        /// </summary>
        /// <param name="serviceType">Service type to find concrete implementations of.</param>
        /// <returns>The concrete implementations of the specified service type.</returns>        
        IEnumerable<Type> GetConcreteImplementionsOf(Type serviceType);

        /// <summary>
        /// Gets the design time generic interface equivalent of the specified run-time generic
        /// interface.
        /// </summary>
        /// <param name="genericServiceType">Run-time generic interface.</param>
        /// <returns>The design time generic interface equivalent of the specified run-time generic
        /// interface.</returns>
        Type GetNonRuntimeGenericInterface(Type genericServiceType);
    }
}
