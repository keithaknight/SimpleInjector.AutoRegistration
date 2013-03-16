using System.Collections.Generic;
using SimpleInjector.AutoRegistration.Contract;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace SimpleInjector.AutoRegistration
{
    public class AutoRegistrationOptions : IAutoRegistrationOptions
    {
        public AutoRegistrationOptions(string rootNamespace)
        {            
            var enabledOptions = new AutoRegistrationEnabledProvider.AttributeAutoRegistrationEnabledProviderOptions(rootNamespace);
            AutoRegistrationEnabledProvider = new AutoRegistrationEnabledProvider.AttributeAutoRegistrationEnabledProvider(enabledOptions);

            ImplementationProvider = new ImplementationProvider.AutoRegistrationImplementationProvider(AutoRegistrationEnabledProvider);

            var attrOptions = new LifestyleResolver.AttributeLifestyleResolverOptions();
            LifestyleResolver = new LifestyleResolver.AttributeLifestyleResolver(ImplementationProvider, attrOptions);

            var catalog = new AssemblyCatalog(this.GetType().Assembly);
            var comp = new CompositionContainer(catalog);            
            comp.ComposeParts(this);
          
            RegisterDecorators = true;
        }
        
        public IAutoRegistrationEnabledProvider AutoRegistrationEnabledProvider { get; set; }
        public IImplementationProvider ImplementationProvider { get; set; }
        public ILifestyleResolver LifestyleResolver { get; set; }

        [ImportMany(typeof(IDependencyRegistrationProvider))]
        public IEnumerable<IDependencyRegistrationProvider> DependencyRegistrationProviders { get; set; }

        public bool RegisterDecorators { get; set; }
    }
}
