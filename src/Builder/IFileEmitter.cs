using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder
{
    [PublicAPI]
    public interface IFileEmitter<TOptions> where TOptions: IEmitterOptions
    {
        void Emit([NotNull]string output, [NotNull]Assembly assembly);
        FileEmitter<TOptions> SetupOptions([NotNull]Action<TOptions> setup);
    }
}