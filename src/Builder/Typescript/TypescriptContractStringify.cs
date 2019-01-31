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
            return $@"{Description(contractDesc)}{Export}interface I{_options.ContractNamingStrategy.GetPropertyName(contractDesc.Name, false)}{Generic(contractDesc)} {{
{string.Join(Environment.NewLine+Environment.NewLine, contractDesc.Fields.Select((field) => Utils.Ident(_fieldStringify.Stringify(field), _options.IdentSize)))}
}}";
        }

        private string Generic(ContractDesc contractDesc)
        {
            var genericsList = string.Join(", ", contractDesc.Generics.Select(_ => _.Name));
            return contractDesc.IsGeneric ? $"<{genericsList}>" : "";
        }

        private string Export => _options.TypescriptModuleType == TypescriptModuleType.Module ? "export " : "";
        
        private string Description(ContractDesc contractDesc) => string.IsNullOrWhiteSpace(contractDesc.Description)
            ? ""
            : $@"// {contractDesc.Description}{Environment.NewLine}";
    }
}