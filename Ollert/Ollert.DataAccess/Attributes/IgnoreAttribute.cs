using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Ezzel az attribútummal megjelölt propertyk nem mentődnek adatbázisba
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}
