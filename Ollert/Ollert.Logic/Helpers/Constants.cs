namespace Ollert.Logic.Helpers
{
    public static class Constants
    {
        /// <summary>
        /// Gyakran használt hibakódok speciális exception kezeléshez.
        /// https://www.postgresql.org/docs/current/errcodes-appendix.html
        /// </summary>
        public static class ErrorCodes
        {
            public const string SqlForeignKeyViolation = "23503";
            public const string SqlUniqueKeyViolation = "23505";
        }
    }
}
