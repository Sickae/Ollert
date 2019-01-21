using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Csak évszámot tartalmazó DateTime típusokhoz
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class DateAttribute : Attribute
    {
    }
}
