using System;
using System.Linq;

namespace SimpleInjector.AutoRegistration.LifestyleResolver
{    
    public class AttributeLifestyleResolverOptions
    {
        public AttributeLifestyleResolverOptions() 
        {            
            ThrowIfLifestyleNotDefined = false;
            DefaultLifestyle = Lifestyle.Transient;
        }

        public bool ThrowIfLifestyleNotDefined { get; set; }
        public SimpleInjector.Lifestyle DefaultLifestyle { get; set; }
    }
}
