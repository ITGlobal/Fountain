using System;
using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    public class GenericDesc: ITypeDesc, IDescriptableDesc, IDeprecatableDesc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public bool CanBePartial { get; set; }
        public IEnumerable<GenericParametrDesc> Arguments { get; set; }
        public Dictionary<string, Attribute> Metadata { get; set; }
        public IEnumerable<ContractFieldDesc> Fields { get; set; }
    }
}