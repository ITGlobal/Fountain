using System;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpContractGenericStringify: IContractGenericStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly CsharpEmitterOptions _options;
        private readonly CsharpTemplateContext _contextMaker;
        private readonly Template _template;

        public CsharpContractGenericStringify(IContractFieldStringify fieldStringify, CsharpEmitterOptions options, CsharpTemplateContext contextMaker)
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
public {{ if can_be_partial }}partial {{ end }}class {{ class_name }}<{{ genericargs }}> {

{{~ for field in fields ~}}
{{ field | ident }}

{{~ end ~}}
}");
        }
        
        public string Stringify(GenericDesc genericDesc)
        {
            return _template.Render(_contextMaker.Make(new
            {
                Genericargs = GenericargsStringify(genericDesc.Arguments),
                genericDesc.Description,
                genericDesc.IsDeprecated,
                genericDesc.DeprecationCause,
                ClassName = _options.ContractNameTempate(genericDesc),
                Fields = genericDesc.Fields.Select(_fieldStringify.Stringify),
                genericDesc.CanBePartial,
            }));
        }

        private string GenericargsStringify(IEnumerable<GenericParametrDesc> args)
        {
            return string.Join(", ", args.Select(_ => _.Name));
        }
    }
}