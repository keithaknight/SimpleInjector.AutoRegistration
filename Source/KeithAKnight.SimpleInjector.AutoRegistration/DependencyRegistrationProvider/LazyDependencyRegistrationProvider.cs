using System;
using System.ComponentModel.Composition;
using System.Linq;
using KeithAKnight.SimpleInjector.AutoRegistration.Contract;
using SimpleInjector;

namespace KeithAKnight.SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    /// <summary>
    /// Provides discovery and registration of the Lazy dependencies to auto-registration enabled
    /// types.
    /// </summary>
    [Export(typeof(IDependencyRegistrationProvider))]
    public class LazyDependencyRegistrationProvider : IDependencyRegistrationProvider
    {       
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LazyDependencyRegistrationProvider()
        {            
        }

        /// <summary>
        /// Discovers any Lazy dependencies of the specified type and attempts to register them.
        /// </summary>
        /// <param name="concreteType">Concrete type for which the dependencies should be discovered
        /// and registered</param>
        /// <param name="container">SimpleInjector container in which the dependencies should be registered.</param>
        /// <param name="options">Options to apply to the discovery and registration process.</param>
        public void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options)
        {
            foreach (var ctor in concreteType.GetConstructors().Where((x) => x.IsPublic))
            {
                foreach (var param in ctor.GetParameters().Where((x) => x.ParameterType.Name.StartsWith("Lazy`")))
                {
                    var funcParamType = param.ParameterType;
                    var genericArgType = funcParamType.GenericTypeArguments.FirstOrDefault();

                    if (genericArgType != null 
                     && genericArgType.IsInterface 
                     && options.AutoRegistrationEnabledProvider.IsAutoRegistrationEnabled(genericArgType))
                    {
                        Lifestyle lifestyle = options.LifestyleResolver.GetLifestyle(genericArgType);

                        if (lifestyle == null)
                        {
                            throw new Exception(string.Format("Lifestyle not defined for wrapped type of Lazy<{0}>", genericArgType.Name));
                        }

                        Func<object> creationDelegate = () =>
                        {
                            //First create a Func<> with the container in context to create an instance of the type
                            var method = typeof(ContextualFuncCreationHelper).GetMethod("GetFunc").MakeGenericMethod(genericArgType);
                            var funcType = typeof(Func<>).MakeGenericType(genericArgType);
                            var funcInstance = new ContextualFuncCreationHelper(container);
                            
                            var dgt = Delegate.CreateDelegate(funcType, funcInstance, method);

                            //Create lazy with the Func delegate as a parameter
                            return Activator.CreateInstance(funcParamType, dgt);
                        };

                        //Since the Lazy<> acts as a singleton and retains state, we need to determine if the Lazy<>
                        //should be transient or singleton
                        if (lifestyle == Lifestyle.Singleton)
                        {
                            container.RegisterSingle(funcParamType, creationDelegate);
                        }
                        else
                        {
                            container.Register(funcParamType, creationDelegate);
                        }
                    }
                }
            }
        }
    }
}
