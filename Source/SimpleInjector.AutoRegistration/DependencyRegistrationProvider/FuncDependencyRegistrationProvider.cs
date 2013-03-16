using System;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    [Export(typeof(IDependencyRegistrationProvider))]
    public class FuncDependencyRegistrationProvider : IDependencyRegistrationProvider
    {    
        public FuncDependencyRegistrationProvider()
        {          
        }

        public void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options)
        {
            foreach (var ctor in concreteType.GetConstructors().Where((x) => x.IsPublic))
            {
                foreach (var param in ctor.GetParameters().Where((x) => x.ParameterType.Name.StartsWith("Func`")))
                {
                    var funcParamType = param.ParameterType;
                    var genericArgType = funcParamType.GenericTypeArguments.FirstOrDefault();

                    if (genericArgType != null 
                     && genericArgType.IsInterface 
                     && options.AutoRegistrationEnabledProvider.IsAutoRegistrationEnabled(genericArgType))
                    {
                        container.RegisterSingle(funcParamType, () =>
                        {
                            var method = typeof(ContextualFuncCreationHelper).GetMethod("GetFunc").MakeGenericMethod(genericArgType);
                            var funcInstance = new ContextualFuncCreationHelper(container);

                            return Delegate.CreateDelegate(funcParamType, funcInstance, method);
                        });
                    }
                }
            }
        }
    }
}
