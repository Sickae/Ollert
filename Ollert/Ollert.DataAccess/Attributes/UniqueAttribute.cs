using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Egyedi propertyk megjelölésére szolgál
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : Attribute
    {
    }
}
