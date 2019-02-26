using System;
using System.Globalization;
using System.Linq;

namespace ITGlobal.Fountain.Builder {
    public static class Utils
    {
        public static string Ident(string str, int ident) {
            var identstr = string.Concat(Enumerable.Repeat(" ", ident));
            var normalized = str.Replace("\n\r", "\n");
            return identstr + normalized.Replace("\n", "\n" + identstr);
        }

        public static string Capitalize(string str)
        {
            return new CultureInfo("en-US", false).TextInfo.ToTitleCase(str);
        }
    }
}