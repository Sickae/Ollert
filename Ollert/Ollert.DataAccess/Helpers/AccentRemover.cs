namespace Ollert.DataAccess.Helpers
{
    public static class AccentRemover
    {
        private const string _accents = "áéíóöőúüűÁÉÍÓÖŐÚÜŰ";
        private const string _accentsReplacements = "aeiooouuuAEIOOOUUU";

        public static string RemoveAccents(string str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                str = str.Replace(_accents[i], _accentsReplacements[i]);
            }
            return str;
        }
    }
}
