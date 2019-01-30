using System.Collections.Generic;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractFieldStringify : IContractFieldStringify
    {
        private readonly IEmitterOptions _options;

        public TypescriptContractFieldStringify(IEmitterOptions options)
        {
            _options = options;
        }

        public string Stringify(ContractFieldDesc fieldDesc)
        {
            return $@"
// {fieldDesc.Description}
{fieldDesc.Name}: {FieldTypeStringify(fieldDesc.Type)}
";
        }

        private string FieldTypeStringify(ITypeDesc type)
        {
            switch (type)
            {
                case ContractDesc t:
                    return $"{t.Name}";
                case PrimitiveTypeDesc t:
                    return $"{t.Key}";
                default:
                    return "any";
            }
        }
    }
}