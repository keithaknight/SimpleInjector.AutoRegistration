using System;
using SimpleInjector;

namespace KeithAKnight.SimpleInjector.AutoRegistration.Contract
{
    /// <summary>
    /// Determines the lifestyle of types.
    /// </summary>
    public interface ILifestyleResolver
    {
        /// <summary>
        /// Determines the lifestyle of the specified type.
        /// </summary>
        /// <param name="type">Type for which the lifestyle should be determined.</param>
        /// <returns>The lifestyle of the specified type.</returns>
        Lifestyle GetLifestyle(Type type);
    }
}
