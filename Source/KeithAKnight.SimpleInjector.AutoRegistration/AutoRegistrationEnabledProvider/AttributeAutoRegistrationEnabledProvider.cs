using System;
using System.Reflection;
using KeithAKnight.SimpleInjector.AutoRegistration.Contract;

namespace KeithAKnight.SimpleInjector.AutoRegistration.AutoRegistrationEnabledProvider
{
    /// <summary>
    /// Provides logic to determine which types should be included or excluded
    /// from the auto-registration process.
    /// </summary>
    public class AttributeAutoRegistrationEnabledProvider : IAutoRegistrationEnabledProvider
    {
        private readonly AttributeAutoRegistrationEnabledProviderOptions options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Options used to detmine which types should be included or excluded
        /// for the auto-registration process.</param>
        public AttributeAutoRegistrationEnabledProvider(AttributeAutoRegistrationEnabledProviderOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Determines if the specified type is enabled for auto-registration.
        /// </summary>
        /// <param name="type">Type to determine if it should be auto-registered.</param>
        /// <returns>A value indicating if the specified type is enabled for
        /// auto-registration</returns>
        public bool IsAutoRegistrationEnabled(Type type)
        {
            return (type.FullName == null || type.FullName.StartsWith(options.RootNamespace))
                && !options.DisabledTypeNames.Contains(type.Name);
        }

        /// <summary>
        /// Determines if the specified assembly is enabled for auto-registration.
        /// </summary>
        /// <param name="assembly">Assembly to determine if it should be auto-registered.</param>
        /// <returns>A value indicating if the specified assembly is enabled for
        /// auto-registration.</returns>
        public bool IsAutoRegistrationEnabled(Assembly assembly)
        {
            return (assembly.FullName == null || assembly.FullName.StartsWith(options.RootNamespace))
                && !options.DisabledTypeNames.Contains(assembly.FullName);
        }       
    }
}
