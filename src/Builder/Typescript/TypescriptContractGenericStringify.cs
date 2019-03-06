using System;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractGenericStringify: IContractGenericStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly TypescriptEmitterOptions _options;
        private readonly TypescriptJsDocComments _jsDoc;

        public TypescriptContractGenericStringify(IContractFieldStringify fieldStringify, TypescriptEmitterOptions options,
            TypescriptJsDocComments jsDoc)
        {
            _fieldStringify = fieldStringify;
            _options = options;
            _jsDoc = jsDoc;
        }

        public string Stringify(GenericDesc contractDesc)
        {
            return
                $@"{_jsDoc.Format(contractDesc)}{Export}interface I{_options.ContractNameTempate(contractDesc)}<{GenericargsStringify(contractDesc.Arguments)}> {{
{string.Join(Environment.NewLine + Environment.NewLine, contractDesc.Fields.Select((field) => Utils.Ident(_fieldStringify.Stringify(field), _options.IdentSize)))}
}}";
        }

        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
        
        private string GenericargsStringify(IEnumerable<GenericParametrDesc> args)
        {
            return string.Join(", ", args.Select(_ => _.Name));
        }
    }
}