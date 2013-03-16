using System;
using System.Linq;
using System.Reflection;
using SimpleInjector.AutoRegistration.Contract;
using SimpleInjector.AutoRegistration.TypeRegistration;

namespace SimpleInjector.AutoRegistration
{
    public static class AutoRegistrationExtension
    {        
        public static void AutoRegister(this Container container, IAutoRegistrationOptions options = null)
        {
            if (options == null)
            {
                string ns = null;

                //Must be in constructor to get actual calling assembly
                var workingAssembly = Assembly.GetEntryAssembly()
                                   ?? Assembly.GetCallingAssembly()
                                   ?? Assembly.GetExecutingAssembly();

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