using System.Reflection;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder
{
    [PublicAPI]
    public interface IFileEmitter
    {
        void Emit(string output, Assembly assembly);
    }
}