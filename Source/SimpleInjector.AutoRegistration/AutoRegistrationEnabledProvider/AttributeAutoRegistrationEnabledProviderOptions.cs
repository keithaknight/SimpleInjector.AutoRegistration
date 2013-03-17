using System.Collections.Generic;

namespace SimpleInjector.AutoRegistration.AutoRegistrationEnabledProvider
{
    /// <summary>
    /// Options used to determine which types should or should not be included in the
    /// auto-registration process.
    /// </summary>
    public class AttributeAutoRegistrationEnabledProviderOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rootNamespace">The root namespace of the application.  By default, 
        /// only types within this root namespace will be auto-registered.</param>
        public AttributeAutoRegistrationEnabledProviderOptions(string rootNamespace)
        {
            DisabledTypeNames = new HashSet<string>();

            RootNamespace = rootNamespace;
        }

        /// <summary>
        /// A collection of names of types or assemblies to exclude from the 
        /// auto-registration process
        /// </summary>
        public HashSet<string> DisabledTypeNames { get; private set; }

        /// <summary>
        /// Gets or sets the root namespace.  By default, only types within this root namespace
        /// will be auto-registered.
        /// </summary>
        public string RootNamespace { get; set; }
    }
}
