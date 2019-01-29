using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    public class ContractDesc: ITypeDesc
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsAbstract { get; set; }
        public IEnumerable<ContractFieldDesc> Fields { get; set; }
        public IEnumerable<ContractGenericDesc> Generics { get; set; }
        public IEnumerable<ContractDesc> Bases { get; set; }
    }
}