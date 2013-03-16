using System;

namespace SimpleInjector.AutoRegistration.Contract
{
    public interface ILifestyleResolver
    {
        Lifestyle GetLifestyle(Type type);
    }
}
