using System.Collections.Generic;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractFieldStringify: IContractFieldStringify
    {
        public string Stringify(ContractFieldDesc fieldDesc, int ident)
        {
            return Utils.Ident($@"
// {fieldDesc.Description}
{fieldDesc.Name}: {FieldTypeStringify(fieldDesc.Type)}
", ident);
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
