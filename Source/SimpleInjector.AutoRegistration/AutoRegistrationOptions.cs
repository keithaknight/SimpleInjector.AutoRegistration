using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using SimpleInjector.AutoRegistration.Contract;

namespace SimpleInjector.AutoRegistration
{
    /// <summary>
    /// Configuration options and extensibility point for configuring Auto-Registration of SimpleInjector.
    /// </summary>
    public class AutoRegistrationOptions : IAutoRegistrationOptions
    {
        /// <summary>
        /// Constructor.  Initializes the class with the default extensibility providers.
        /// </summary>
        /// <param name="rootNamespace">Root namespace of the application.  All auto-registration will be restricted to 
        /// types within this namespace.</param>
        public AutoRegistrationOptions(string rootNamespace)
        { 
            var enabledOptions = new AutoRegistrationEnabledProvider.AttributeAutoRegistrationEnabledProviderOptions(rootNamespace);
            AutoRegistrationEnabledProvider = new AutoRegistrationEnabledProvider.AttributeAutoRegistrationEnabledProvider(enabledOptions);

            ImplementationProvider = new ImplementationProvider.AutoRegistrationImplementationProvider(AutoRegistrationEnabledProvider);

            var attrOptions = new LifestyleResolver.AttributeLifestyleResolverOptions();
            LifestyleResolver = new LifestyleResolver.AttributeLifestyleResolver(ImplementationProvider, attrOptions);

            RegisterMefExtensions();
          
            RegisterDecorators = true;
        }

        /// <summary>
        /// Discovers and registers all Microsoft Extensibility Framework services registered in this
        /// assembly.
        /// </summary>
        private void RegisterMefExtensions()
        {
            var catalog = new AssemblyCatalog(this.GetType().Assembly);
            var comp = new CompositionContainer(catalog);            
            comp.ComposeParts(this);
        }
        
        /// <summary>
        /// Controls which types will be auto-registered.  By default, all types in the application root
        /// namespace will be registered unless otherwise specified.  When using the built-in provider, you may
        /// specify type or assembly names to exclude from auto-registration.  Custom providers can be specified 
        /// to replace or extend this one.
        /// </summary>
        public IAutoRegistrationEnabledProvider AutoRegistrationEnabledProvider { get; set; }

        /// <summary>
        /// Provides the services and concrete implementations to auto-register.  Custom providers
        /// can be specified to replace or extend this one.
        /// </summary>
        public IImplementationProvider ImplementationProvider { get; set; }

        /// <summary>
        /// Resolves the lifestyle of types for auto-registration.  By default, all types will be transient unless 
        /// they have an ImmutableObjectAttribute that specifies that the type is immutable.  Custom resolvers can be specified
        /// to replace or extend this one.
        /// </summary>
        public ILifestyleResolver LifestyleResolver { get; set; }

        /// <summary>
        /// Providers for discovering and registering dependencies of each type.  This collection can be replaced or extended with
        /// custom providers.
        /// </summary>
        [ImportMany(typeof(IDependencyRegistrationProvider))]
        public IEnumerable<IDependencyRegistrationProvider> DependencyRegistrationProviders { get; set; }

        /// <summary>
        /// Gets or sets if decorators should be discovered and automatically registered.
        /// </summary>
        public bool RegisterDecorators { get; set; }
    }
}
