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
        private readonly TypescriptJsDocComments _jsDoc;

        public TypescriptContractStringify(IContractFieldStringify fieldStringify, TypescriptEmitterOptions options,
            TypescriptJsDocComments jsDoc)
        {
            _fieldStringify = fieldStringify;
            _options = options;
            _jsDoc = jsDoc;
        }

        public string Stringify(ContractDesc contractDesc)
        {
            return
                $@"{_jsDoc.Format(contractDesc)}{Export}interface I{_options.ContractNameTempate(contractDesc)} {{
{string.Join(Environment.NewLine + Environment.NewLine, contractDesc.Fields.Select((field) => Utils.Ident(_fieldStringify.Stringify(field), _options.IdentSize)))}
}}";
        }

        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
    }
}