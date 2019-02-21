using System.Collections.Generic;
using ITGlobal.Fountain.Parser.Validation;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Parser
{
    public class ContractFieldDesc: IDeprecatableDesc, IDescriptableDesc
    {
        public string Name { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public string Description { get; set; }
        public bool MayBeMissing { get; set; }
        [CanBeNull] public string JsonName { get; set; }
        public ITypeDesc Type { get; set; }
        public IEnumerable<IFieldValidation> Validation { get; set; }
        public string Example { get; set; }
    }
}