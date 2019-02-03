using System;
using ITGlobal.Fountain.Annotations;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Parser
{
    public interface IParserOptions
    {
        [NotNull] Func<ContractAttribute, bool> FilterContracts { get; set; }
        [NotNull] Func<ContractFieldAttribute, bool> FilterFields { get; set; }
        [NotNull] Func<string, bool> ValidationDestinationFilter { get; set; }
    }
}