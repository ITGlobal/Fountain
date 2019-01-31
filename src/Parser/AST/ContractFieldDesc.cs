using System.Collections;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ITGlobal.Fountain.Parser
{
    public class ContractFieldDesc
    {
        public string Name { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public string Description { get; set; }
        public bool ContainGenerics { get; set; }
        public IEnumerable<ContractGenericDesc>  { get; set; }
        [CanBeNull] public JsonPropertyAttribute JsonProperty { get; set; }
        public ITypeDesc Type { get; set; }
    }
}