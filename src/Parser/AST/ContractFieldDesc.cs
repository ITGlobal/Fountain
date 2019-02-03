using System.Collections;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ITGlobal.Fountain.Parser
{
    public class ContractFieldDesc: IDeprecatableDesc, IDescriptableDesc
    {
        public string Name { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public string Description { get; set; }
        public bool MayBeMissing { get; set; }
        [CanBeNull] public string JsonProperty { get; set; }
        public ITypeDesc Type { get; set; }
    }
}