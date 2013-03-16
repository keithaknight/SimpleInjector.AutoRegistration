using System;

namespace SimpleInjector.AutoRegistration.Contract
{
    public interface IDependencyRegistrationProvider
    {
        void RegisterDependencies(Type concreteType, Container container, IAutoRegistrationOptions options);
    }
}
