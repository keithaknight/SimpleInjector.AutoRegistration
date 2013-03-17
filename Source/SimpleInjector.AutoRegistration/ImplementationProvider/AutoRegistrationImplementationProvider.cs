using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration.ImplementationProvider
{
    /// <summary>
    /// Discovers the appropriate concrete implementations and services to be auto-registered.
    /// </summary>
    public class AutoRegistrationImplementationProvider : IImplementationProvider
    {
        private readonly IDictionary<Type, List<Type>> resolvedTypes;
        private readonly Lazy<IEnumerable<Assembly>> assemblies;
        private readonly IAutoRegistrationEnabledProvider enabledProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="enabledProvider">A provider that determines which types allow for auto-registration.</param>
        public AutoRegistrationImplementationProvider(IAutoRegistrationEnabledProvider enabledProvider)
        {
            this.enabledProvider = enabledProvider;
            resolvedTypes = new Dictionary<Type, List<Type>>();

            this.assemblies = new Lazy<IEnumerable<Assembly>>(() => AppDomain.CurrentDomain.GetAssemblies().Where((x) => enabledProvider.IsAutoRegistrationEnabled(x)).ToArray());
        }

        /// <summary>
        /// Gets the concrete implementations of the specified service type.
        /// </summary>
        /// <param name="serviceType">Service type to find concrete implementations of.</param>
        /// <returns>The concrete implementations of the specified service type.</returns>
        public IEnumerable<Type> GetConcreteImplementionsOf(Type serviceType)
        {
            List<Type> list;
            if (!resolvedTypes.TryGetValue(serviceType, out list))
            {
                list = new List<Type>();

                foreach (var assembly in this.assemblies.Value)
                {
                    list.AddRange(GetAssemblyExportedTypes(assembly).Where((x) => x.IsClass
                        && !x.IsAbstract
                        && serviceType.IsAssignableFrom(x)));
                }

                resolvedTypes[serviceType] = list;
            }
            
            return list;
        }

        /// <summary>
        /// Gets all concrete types that have auto-registration enabled.
        /// </summary>
        /// <returns>All concrete types that have auto-registration enabled.</returns>
        public IEnumerable<Type> GetAllConcreteTypes()
        {
            var list = new List<Type>();

            foreach (var assembly in this.assemblies.Value)
            {
                list.AddRange(GetAssemblyExportedTypes(assembly).Where((x) => !x.IsSubclassOf(typeof(System.Attribute)) && !x.IsInterface));                    
            }

            return list;
        }

        /// <summary>
        /// Gets the design time generic interface equivalent of the specified run-time generic
        /// interface.
        /// </summary>
        /// <param name="genericServiceType">Run-time generic interface.</param>
        /// <returns>The design time generic interface equivalent of the specified run-time generic
        /// interface.</returns>
        public Type GetNonRuntimeGenericInterface(Type genericServiceType)
        {
            Type foundType = null;

            foreach (var assembly in this.assemblies.Value)
            {
                foundType = GetAssemblyExportedTypes(assembly).Where((x) => x.IsInterface                      
                    && string.Compare(genericServiceType.Name, x.Name, false) == 0).FirstOrDefault();
                
                if (foundType != null) break;
            }

            return foundType;
        }

        /// <summary>
        /// Discovers and returns all auto-registration enabled types in the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to search for auto-registration enabled types.</param>
        /// <returns>All auto-registration enabled types in the specified assembly.</returns>
        private IEnumerable<Type> GetAssemblyExportedTypes(Assembly assembly)
        {
            return assembly.GetExportedTypes().Where((x) => this.enabledProvider.IsAutoRegistrationEnabled(x));
        }
    }
}
