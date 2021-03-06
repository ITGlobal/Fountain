using System.Collections.Generic;
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
{{~ for c in custom_attrs ~}}
[{{ c }}]
{{~ end ~}}
public {{ if can_be_partial }}partial {{ end }}class {{ class_name }}{{ if has_base }}: {{ base_class }}{{ end }} {

{{~ for field in fields ~}}
{{ field | ident }}

{{~ end ~}}
}");
        }
        
        public string Stringify(ContractDesc contractDesc)
        {
            var baseClass = contractDesc.Base == null ? null : _fieldStringify.FieldTypeStringify(contractDesc.Base);
            return _template.Render(_contextMaker.Make(new
            {
                contractDesc.Description,
                contractDesc.IsDeprecated,
                contractDesc.DeprecationCause,
                ClassName = _options.ContractNameTempate(contractDesc),
                Fields = contractDesc.Fields.Select(_fieldStringify.Stringify),
                contractDesc.CanBePartial,
                HasBase = contractDesc.Base != null, 
                BaseClass = baseClass,
                CustomAttrs = contractDesc.CustomAttributes.Select(_ => _.AttributeStr)
            }));
        }
    }
}