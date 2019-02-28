using System;
using ITGlobal.Fountain.Annotations;

namespace ITGlobal.Fountain.Parser
{
    public class ParserOptionsDefault : IParserOptions
    {
        public Func<ContractAttribute, bool> FilterContracts { get; set; } = (attr) => true;
        public Func<ContractFieldAttribute, bool> FilterFields { get; set; } = (attr) => true;
        public Func<string, bool> ValidationDestinationFilter { get; set; } = dest => true;
        public Func<string, bool> CustomAttrDestinationFilter { get; set; } = dest => true;
    }
}