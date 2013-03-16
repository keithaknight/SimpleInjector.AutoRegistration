using System.Collections.Generic;

namespace SimpleInjector.AutoRegistration.AutoRegistrationEnabledProvider
{
    public class AttributeAutoRegistrationEnabledProviderOptions
    {
        public AttributeAutoRegistrationEnabledProviderOptions(string rootNamespace)
        {
            DisabledTypeNames = new HashSet<string>();

            RootNamespace = rootNamespace;
        }

        public HashSet<string> DisabledTypeNames { get; private set; }
        public string RootNamespace { get; set; }
    }
}
