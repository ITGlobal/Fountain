using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace Builder.Typescript
{
    public class TypescriptContractStringify: IContractStringify
    {
//        public const string TEMPLATE = @"
//interface I{{ contract.Name }} {
//    {{~ for field in contract.Fields ~}}
//    {{ field.Key }}: {{ field.Value  }} 
//    {{~ endfor ~}}
//}
//"
        
        public string Stringify(ContractDesc contractDesc)
        {
            return $@"
interface I{contractDesc.Name} {{
    {string.Join("\n\n", contractDesc.Fields.Select(StringifyField))}              
}}
";
        }

        private string StringifyField(KeyValuePair<string, ContractFieldDesc> pair)
        {
            return $@"
// {pair.Value.Description}
{pair.Key}: {pair.Value.Type}
";
        }
    }
}