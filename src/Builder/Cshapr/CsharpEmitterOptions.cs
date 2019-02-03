using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpEmitterOptions : EmitterOptions
    {
        [NotNull] public Func<string, ITypeDesc, string> CsharpNamespaceTemplatePerFile { get; set; } = 
            (group, desc) => $"Contracts.{Utils.Capitalize(group)}";

        [NotNull] public string CsharpNamespaceOneFile { get; set; } = "Contracts";
    }
}