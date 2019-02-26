using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpEmitterOptions : EmitterOptions
    {
        /// <summary>
        /// Set namespace template for perfile mode
        /// </summary>
        [NotNull] public Func<string, ITypeDesc, string> CsharpNamespaceTemplatePerFile { get; set; } = 
            (group, desc) => $"Contracts.{Utils.Capitalize(group)}";

        /// <summary>
        /// set namespace name for one file mode
        /// </summary>
        [NotNull] public string CsharpNamespaceOneFile { get; set; } = "Contracts";
    }
}