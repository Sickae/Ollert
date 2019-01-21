using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Az osztály komponensként való mappelésére szolgál
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentTypeAttribute : Attribute
    {
    }
}
