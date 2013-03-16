using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration.LifestyleResolver
{
    public class AttributeLifestyleResolver : ILifestyleResolver
    {
        private readonly IDictionary<Type, Lifestyle> resolvedLifestyles;
        private readonly IImplementationProvider implementationProvider;
        private readonly AttributeLifestyleResolverOptions options;

        public AttributeLifestyleResolver(IImplementationProvider implementationProvider, AttributeLifestyleResolverOptions options)
        {
            this.implementationProvider = implementationProvider;
            this.options = options;

            this.resolvedLifestyles = new Dictionary<Type, Lifestyle>();
        }

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
