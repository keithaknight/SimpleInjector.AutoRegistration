using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration.ImplementationProvider
{
    public class AutoRegistrationImplementationProvider : IImplementationProvider
    {
        private readonly IDictionary<Type, List<Type>> resolvedTypes;
        private readonly Lazy<IEnumerable<Assembly>> assemblies;
        private readonly IAutoRegistrationEnabledProvider enabledProvider;

        public AutoRegistrationImplementationProvider(IAutoRegistrationEnabledProvider enabledProvider)
        {
            this.enabledProvider = enabledProvider;
            resolvedTypes = new Dictionary<Type, List<Type>>();

            this.assemblies = new Lazy<IEnumerable<Assembly>>(() => AppDomain.CurrentDomain.GetAssemblies().Where((x) => enabledProvider.IsAutoRegistrationEnabled(x)).ToArray());
        }

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

        public IEnumerable<Type> GetAllConcreteTypes()
        {
            var list = new List<Type>();

            foreach (var assembly in this.assemblies.Value)
            {
                list.AddRange(GetAssemblyExportedTypes(assembly).Where((x) => !x.IsSubclassOf(typeof(System.Attribute)) && !x.IsInterface));                    
            }

            return list;
        }

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

        private IEnumerable<Type> GetAssemblyExportedTypes(Assembly assembly)
        {
            return assembly.GetExportedTypes().Where((x) => this.enabledProvider.IsAutoRegistrationEnabled(x));
        }
    }
}
