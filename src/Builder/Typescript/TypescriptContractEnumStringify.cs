using System;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractEnumStringify : IContractEnumStringify
    {
        private readonly IEnumFieldStringify _enumFieldStringify;
        private readonly TypescriptEmitterOptions _options;
        private readonly TypescriptJsDocComments _jsDoc;

        public TypescriptContractEnumStringify(IEnumFieldStringify enumFieldStringify, TypescriptEmitterOptions options,
            TypescriptJsDocComments jsDoc)
        {
            _enumFieldStringify = enumFieldStringify;
            _options = options;
            _jsDoc = jsDoc;
        }

        public string Stringify(ContractEnumDesc contractDesc)
        {
            return
                $@"{_jsDoc.Format(contractDesc)}{Export}type I{_options.ContractNameTempate(contractDesc)} =
{string.Join(Environment.NewLine, contractDesc.Values.Select(field => Utils.Ident(_enumFieldStringify.Stringify(field), _options.IdentSize)))}";
        }

        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
    }
}