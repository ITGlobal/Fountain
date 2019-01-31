using System;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractEnumStringify: IContractEnumStringify
    {
        private readonly IEnumFieldStringify _enumFieldStringify;
        private readonly TypescriptEmitterOptions _options;

        public TypescriptContractEnumStringify(IEnumFieldStringify enumFieldStringify, TypescriptEmitterOptions options)
        {
            _enumFieldStringify = enumFieldStringify;
            _options = options;
        }
        
        public string Stringify(ContractEnumDesc contractDesc)
        {
            return $@"
{Export}type I{_options.ContractNamingStrategy.GetPropertyName(contractDesc.Name, false)} =
{string.Join(Environment.NewLine, contractDesc.Values.Select((field) => Utils.Ident(_enumFieldStringify.Stringify(field), _options.IdentSize)))}
";
        }
        
        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
    }
}