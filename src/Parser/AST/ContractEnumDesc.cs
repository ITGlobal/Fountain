using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    public class ContractEnumDesc: ITypeDesc
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<EnumValueDesc> Values { get; set; }
        public bool IsDeprecated { get; set; }
    }
}