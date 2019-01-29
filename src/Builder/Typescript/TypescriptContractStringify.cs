using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractStringify : IContractStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        public TypescriptContractStringify(IContractFieldStringify fieldStringify)
        {
            this._fieldStringify = fieldStringify;
        }

        public string Stringify(ContractDesc contractDesc, int ident)
        {
            return Utils.Ident($@"
interface I{contractDesc.Name} {{
{string.Join("\n\n", contractDesc.Fields.Select((field) => this._fieldStringify.Stringify(field, ident+4)))}
}}
", ident);
        }
    }
}