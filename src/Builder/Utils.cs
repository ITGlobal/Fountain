using System.Linq;

namespace ITGlobal.Fountain.Builder {
    public static class Utils
    {
        public static string Ident(int ident) {
            return string.Concat(Enumerable.Repeat(" ", ident));
        }
    }
}