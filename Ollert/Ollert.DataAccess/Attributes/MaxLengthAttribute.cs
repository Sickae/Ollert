using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Adatmező maximális mérete állítható vele
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLengthAttribute : Attribute
    {
        public int MaxLength { get; set; }

        public MaxLengthAttribute()
        {
            MaxLength = int.MaxValue;
        }

        public MaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
    }
}
