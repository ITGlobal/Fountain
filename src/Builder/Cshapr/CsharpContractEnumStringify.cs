using System.Linq;
using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpContractEnumStringify: IContractEnumStringify
    {
        private readonly IEnumFieldStringify _fieldStringify;
        private readonly CsharpEmitterOptions _options;
        private readonly CsharpTemplateContext _contextMaker;
        private readonly Template _template;

        public CsharpContractEnumStringify(IEnumFieldStringify fieldStringify, CsharpEmitterOptions options, CsharpTemplateContext contextMaker)
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
{{~ if json_converter ~}}
[JsonConverter(typeof({{ json_converter }}))]
{{~ end ~}}
public enum {{ enum_name }} {

{{~ for field in fields ~}}
{{ field | ident }}

{{~ end ~}}
}");
        }
        
        public string Stringify(ContractEnumDesc contractDesc)
        {
            return _template.Render(_contextMaker.Make(new
            {
                contractDesc.Description,
                contractDesc.IsDeprecated,
                contractDesc.DeprecationCause,
                JsonConverter = contractDesc.JsonConverterType?.Name,
                EnumName = _options.ContractNameTempate(contractDesc),
                Fields = contractDesc.Values.Select(_fieldStringify.Stringify),
            }));
        }
    }
}