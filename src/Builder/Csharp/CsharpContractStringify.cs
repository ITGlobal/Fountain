using System.Linq;
using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpContractStringify: IContractStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly CsharpEmitterOptions _options;
        private readonly CsharpTemplateContext _contextMaker;
        private Template _template;

        public CsharpContractStringify(IContractFieldStringify fieldStringify, CsharpEmitterOptions options, CsharpTemplateContext contextMaker)
        {
            _fieldStringify = fieldStringify;
            _options = options;
            _contextMaker = contextMaker;
            _template = Template.Parse(@"{{~ if description ~}}
/// <summary>
/// {{ description }}
/// </summary>
{{~ end ~}}
{{~ if is_deprecated ~}}
[Obsolete(""{{ deprecation_cause }}"")]
{{~ end ~}}
public class {{ class_name }} {

{{~ for field in fields ~}}
{{ field | ident }}

{{~ end ~}}
}");
        }
        
        public string Stringify(ContractDesc contractDesc)
        {
            return _template.Render(_contextMaker.Make(new
            {
                contractDesc.Description,
                contractDesc.IsDeprecated,
                contractDesc.DeprecationCause,
                ClassName = _options.ContractNameTempate(contractDesc),
                Fields = contractDesc.Fields.Select(_fieldStringify.Stringify),
            }));
        }
    }
}