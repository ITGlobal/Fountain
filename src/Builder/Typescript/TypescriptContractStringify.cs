using System;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractStringify : IContractStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly TypescriptEmitterOptions _options;

        public TypescriptContractStringify(IContractFieldStringify fieldStringify, TypescriptEmitterOptions options)
        {
            _fieldStringify = fieldStringify;
            _options = options;
        }

        public string Stringify(ContractDesc contractDesc)
        {
            return $@"
{Export}interface I{_options.ContractNamingStrategy.GetPropertyName(contractDesc.Name, false) } {{
{string.Join(Environment.NewLine, contractDesc.Fields.Select((field) => Utils.Ident(_fieldStringify.Stringify(field), _options.IdentSize)))}
}}
";
        }

        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
    }
}