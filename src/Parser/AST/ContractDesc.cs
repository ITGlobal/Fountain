using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Parser
{
    public class ContractDesc: ITypeDesc, IDeprecatableDesc, IDescriptableDesc
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContractFieldDesc> Fields { get; set; }
        [CanBeNull] public ITypeDesc Base { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public Dictionary<string, Attribute> Metadata { get; set; }
    }
}