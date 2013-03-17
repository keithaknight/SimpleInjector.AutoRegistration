using System;
using System.Linq;
using System.Reflection;
using SimpleInjector.AutoRegistration.Contract;
using SimpleInjector.AutoRegistration.TypeRegistration;

namespace SimpleInjector.AutoRegistration
{
    /// <summary>
    /// Static container for Auto-Registration extension methods
    /// </summary>
    public static class AutoRegistrationExtension
    {        
        /// <summary>
        /// Discovers and registers all types within the root namespace of the application.  To extend the behavior or specify 
        /// details, including the root namespace, please specify the options object.
        /// </summary>
        /// <param name="container">SimpleInjector container in which these types should be registered.</param>
        /// <param name="options">Optional: options to use while registering types.  This can also be used or override or
        /// extend the behavior of the auto-registration process.</param>
        public static void AutoRegister(this Container container, IAutoRegistrationOptions options = null)
        {
            if (options == null)
            {
                string ns = null;

                //Must be in constructor to get actual calling assembly
                var workingAssembly = Assembly.GetEntryAssembly()
                                   ?? Assembly.GetCallingAssembly();

                if (workingAssembly != null)
                {
                    var workingNamespace = workingAssembly.FullName;
                    var rootNamespaceDotIndex = workingNamespace.IndexOf(".");

                    ns = rootNamespaceDotIndex > 0 ? workingNamespace.Substring(0, rootNamespaceDotIndex) : workingNamespace;
                }

                options = new AutoRegistrationOptions(ns);
            }            
                      
            var typeRegProvider = new TypeRegistrationProvider(container, options);
            typeRegProvider.RegisterTypes();
        }               
    }
}