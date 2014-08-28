using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KeithAKnight.SimpleInjector.AutoRegistration.Contract;
using SimpleInjector;

namespace KeithAKnight.SimpleInjector.AutoRegistration.LifestyleResolver
{
    /// <summary>
    /// Determines the lifestyle of types based on associated attributes.
    /// </summary>
    public class AttributeLifestyleResolver : ILifestyleResolver
    {
        private readonly IDictionary<Type, Lifestyle> resolvedLifestyles;
        private readonly IImplementationProvider implementationProvider;
        private readonly AttributeLifestyleResolverOptions options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="implementationProvider">The type implementation provider to use.</param>
        /// <param name="options">The options used to determine the lifestyle of types.</param>
        public AttributeLifestyleResolver(IImplementationProvider implementationProvider, AttributeLifestyleResolverOptions options)
        {
            this.implementationProvider = implementationProvider;
            this.options = options;

            this.resolvedLifestyles = new Dictionary<Type, Lifestyle>();
        }

        /// <summary>
        /// Determines the lifestyle of the specified type.
        /// </summary>
        /// <param name="type">Type for which the lifestyle should be determined.</param>
        /// <returns>The lifestyle of the specified type.</returns>
        public Lifestyle GetLifestyle(Type type)
        {
            Lifestyle lifestyle;

            if (type.IsInterface)
            {
                var implementations = implementationProvider.GetConcreteImplementionsOf(type);
                type = implementations.FirstOrDefault();
            }

            if (!this.resolvedLifestyles.TryGetValue(type, out lifestyle))
            {
                lifestyle = GetLifestyleFromAttributes(type.GetCustomAttributes(typeof(ImmutableObjectAttribute), false))
                         ?? GetLifestyleFromAttributes(type.Assembly.GetCustomAttributes(typeof(ImmutableObjectAttribute), false));

                if (lifestyle == null)
                {
                    if (this.options.ThrowIfLifestyleNotDefined)
                    {
                        throw new NotSupportedException("Lifestyle not defined for type: " + type.FullName);
                    }
                    else
                    {
                        lifestyle = this.options.DefaultLifestyle;
                    }
                }

                this.resolvedLifestyles[type] = lifestyle;
            }
            
            return lifestyle;
        }

        /// <summary>
        /// Determines the lifestyle based on the specified attributes
        /// </summary>
        /// <param name="attrs">Array of attributes associated with the type</param>
        /// <returns>A value indicating the lifestyle indicated by the specified attributes.</returns>
        private Lifestyle GetLifestyleFromAttributes(object[] attrs)
        {
            var immutableObj = attrs.FirstOrDefault() as ImmutableObjectAttribute;
            bool? isImmutable = immutableObj != null ? (bool?)immutableObj.Immutable : null;

            Lifestyle lifestyle;

            if (!isImmutable.HasValue)
            {
                lifestyle = null;
            }
            else
            {
                lifestyle = isImmutable.Value ? Lifestyle.Singleton : Lifestyle.Transient;
            }

            return lifestyle;
        }
    }
}
