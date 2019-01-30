using System;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractStringify : IContractStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly IEmitterOptions _options;

        public TypescriptContractStringify(IContractFieldStringify fieldStringify, IEmitterOptions options)
        {
            _fieldStringify = fieldStringify;
            _options = options;
        }

        public string Stringify(ContractDesc contractDesc)
        {
            return $@"
interface I{contractDesc.Name} {{
{string.Join(Environment.NewLine, contractDesc.Fields.Select((field) => Utils.Ident(_fieldStringify.Stringify(field), _options.IdentSize)))}
}}
";
        }
    }
}