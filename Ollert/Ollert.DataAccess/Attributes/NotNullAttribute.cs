using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Nem enged nullt tárolni az adatbázisba
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class NotNullAttribute : Attribute
    {
    }
}
