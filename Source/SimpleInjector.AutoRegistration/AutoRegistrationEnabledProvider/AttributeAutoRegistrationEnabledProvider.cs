using System;
using System.Reflection;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration.AutoRegistrationEnabledProvider
{
    public class AttributeAutoRegistrationEnabledProvider : IAutoRegistrationEnabledProvider
    {
        private readonly AttributeAutoRegistrationEnabledProviderOptions options;

        public AttributeAutoRegistrationEnabledProvider(AttributeAutoRegistrationEnabledProviderOptions options)
        {
            this.options = options;
        }

        public bool IsAutoRegistrationEnabled(Type type)
        {
            return (type.FullName == null || type.FullName.StartsWith(options.RootNamespace))
                && !options.DisabledTypeNames.Contains(type.Name);
        }

        public bool IsAutoRegistrationEnabled(Assembly assembly)
        {
            return (assembly.FullName == null || assembly.FullName.StartsWith(options.RootNamespace))
                && !options.DisabledTypeNames.Contains(assembly.FullName);
        }       
    }
}
