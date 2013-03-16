using System;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    [Export(typeof(IDependencyRegistrationProvider))]
    public class LazyDependencyRegistrationProvider : IDependencyRegistrationProvider
    {       
        public LazyDependencyRegistrationProvider()
        {            
        }

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
                            var method = typeof(ContextualFuncCreationHelper).GetMethod("GetFunc").MakeGenericMethod(genericArgType);
                            var funcType = typeof(Func<>).MakeGenericType(genericArgType);
                            var funcInstance = new ContextualFuncCreationHelper(container);

                            var dgt = Delegate.CreateDelegate(funcType, funcInstance, method);

                            return Activator.CreateInstance(funcParamType, dgt);
                        };

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
