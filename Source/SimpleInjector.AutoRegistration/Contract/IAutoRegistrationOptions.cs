using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleInjector.AutoRegistration.Contract
{
    public interface IAutoRegistrationOptions
    {
        IAutoRegistrationEnabledProvider AutoRegistrationEnabledProvider { get; set; }
        IImplementationProvider ImplementationProvider { get; set; }
        ILifestyleResolver LifestyleResolver { get; set; }
        IEnumerable<IDependencyRegistrationProvider> DependencyRegistrationProviders { get; set; }

        bool RegisterDecorators { get; set; }
    }
}
