using System;

namespace Ollert.DataAccess.Attributes
{
    /// <summary>
    /// NHibernate mappinghez segítő attribute
    /// </summary>
    /// <remarks>
    /// Számított értékek (pl. életkor) való ignorálása szolgál mappingnél
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FormulaAttribute : Attribute
    {
        public string Formula { get; set; }

        public string[] FormulaArgs { get; set; }

        public FormulaAttribute(string formula, params string[] args)
        {
            Formula = formula;
            FormulaArgs = args;
        }
    }
}
