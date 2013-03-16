using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleInjector.AutoRegistration.Contract
{
    public interface IImplementationProvider
    {
        IEnumerable<Type> GetAllConcreteTypes();
        IEnumerable<Type> GetConcreteImplementionsOf(Type serviceType);
        Type GetNonRuntimeGenericInterface(Type genericServiceType);
    }
}
