using System.Collections.Generic;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractFieldStringify: IContractFieldStringify
    {
        public string Stringify(ContractFieldDesc fieldDesc, int ident)
        {
            return $@"
{Utils.Ident(ident)}// {fieldDesc.Description}
{Utils.Ident(ident)}{fieldDesc.Name}: {fieldDesc.Type}
";
        }
    }
}
