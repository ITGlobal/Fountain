using System;
using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    public class ContractEnumDesc: ITypeDesc, IDeprecatableDesc, IDescriptableDesc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<EnumValueDesc> Values { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public Type JsonConverterType { get; set; }
    }
}