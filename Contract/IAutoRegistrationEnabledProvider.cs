using System;
using System.Reflection;

namespace SimpleInjector.AutoRegistration.Contract
{
    public interface IAutoRegistrationEnabledProvider
    {
        bool IsAutoRegistrationEnabled(Type type);
        bool IsAutoRegistrationEnabled(Assembly assembly);
    }
}
