using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    public class ContractGroup
    {
        public Dictionary<string, IEnumerable<ITypeDesc>> Groups { get; set; }
    }
}