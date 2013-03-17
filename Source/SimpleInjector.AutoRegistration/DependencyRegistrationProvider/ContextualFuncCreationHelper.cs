using System;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    /// <summary>
    /// A simple helper that can exposes a Func that creates instances
    /// based on the contextual container.
    /// </summary>
    public class ContextualFuncCreationHelper
    {
        private readonly Container container;

        /// <summary>
        /// Constructor.  Takes the container to keep it in context for the
        /// GetFunc method.
        /// </summary>
        /// <param name="container">SimpleInjector Container instance.</param>
        public ContextualFuncCreationHelper(Container container)
        {
            this.container = container;
        }

        /// <summary>
        /// Publicly exposed Func method that will use the contextual SimpleInjector
        /// Container instance to create instances of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <returns>A new instance of T, as defined by the registration rules
        /// of the SimpleInjector container.</returns>
        public T GetFunc<T>() where T : class
        {
            return container.GetInstance<T>();
        }
    }
}
