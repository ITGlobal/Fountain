using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpEmitterOptions : EmitterOptions
    {
        [NotNull] public Func<string, ITypeDesc, string> CsharpNamespaceTemplate { get; set; } = (group, desc) => $"Contracts.{group}";
    }
}