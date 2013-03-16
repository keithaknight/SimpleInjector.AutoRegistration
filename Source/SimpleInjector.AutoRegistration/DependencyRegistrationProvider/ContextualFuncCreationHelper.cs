using System;

namespace SimpleInjector.AutoRegistration.DependencyRegistrationProvider
{
    public class ContextualFuncCreationHelper
    {
        private readonly Container container;

        public ContextualFuncCreationHelper(Container container)
        {
            this.container = container;
        }

        public T GetFunc<T>() where T : class
        {
            return container.GetInstance<T>();
        }
    }
}
