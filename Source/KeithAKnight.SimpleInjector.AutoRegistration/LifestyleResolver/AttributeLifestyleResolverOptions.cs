using System;
using System.Linq;
using SimpleInjector;

namespace KeithAKnight.SimpleInjector.AutoRegistration.LifestyleResolver
{    
    /// <summary>
    /// Options used to resolve lifestyle based on type attributes
    /// </summary>
    public class AttributeLifestyleResolverOptions
    {
        /// <summary>
        /// Constructor.  Creates a new AttributeLifestyleResolverOptions instance.
        /// </summary>
        public AttributeLifestyleResolverOptions() 
        {            
            ThrowIfLifestyleNotDefined = false;
            DefaultLifestyle = Lifestyle.Transient;
        }

        /// <summary>
        /// Gets or sets a value indicating if an exception should be thrown in the lifestyle 
        /// cannot be explicitly determined for a type.
        /// </summary>
        public bool ThrowIfLifestyleNotDefined { get; set; }

        /// <summary>
        /// Gets or sets the default lifestyle to use when the lifestyle cannot be explicitly
        /// determined for the type.
        /// </summary>
        public Lifestyle DefaultLifestyle { get; set; }
    }
}
