using System;
using System.Reflection;

namespace KeithAKnight.SimpleInjector.AutoRegistration.Contract
{
    /// <summary>
    /// Provides logic to determine which types should be included or excluded
    /// from the auto-registration process.
    /// </summary>
    public interface IAutoRegistrationEnabledProvider
    {
        /// <summary>
        /// Determines if the specified type is enabled for auto-registration.
        /// </summary>
        /// <param name="type">Type to determine if it should be auto-registered.</param>
        /// <returns>A value indicating if the specified type is enabled for
        /// auto-registration</returns>
        bool IsAutoRegistrationEnabled(Type type);

        /// <summary>
        /// Determines if the specified assembly is enabled for auto-registration.
        /// </summary>
        /// <param name="assembly">Assembly to determine if it should be auto-registered.</param>
        /// <returns>A value indicating if the specified assembly is enabled for
        /// auto-registration.</returns>
        bool IsAutoRegistrationEnabled(Assembly assembly);
    }
}
