using System;
using System.Reflection;
using ITGlobal.Fountain.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scriban;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpEnumFieldStringify: IEnumFieldStringify
    {
        private readonly CsharpEmitterOptions _options;
        private readonly Template _template;

        public CsharpEnumFieldStringify(CsharpEmitterOptions options)
        {
            _options = options;
            _template = Template.Parse(
                @"{{~ if description ~}}
/// <summary>
/// {{ description }}
/// </summary>
{{~ end ~}}
{{~ if is_deprecated ~}}
[Obsolete(""{{ deprecation_cause }}"")]
{{~ end ~}}
{{~ if enum_member ~}}
[EnumMember(Value = ""{{ enum_member }}"")]
{{~ end ~}}
{{name}},");
        }
        
        public string Stringify(EnumValueDesc field)
        {
            return _template.Render(new
            {
                field.Description,
                field.IsDeprecated,
                field.DeprecationCause,
                field.EnumMember,
                Name = field.Value.ToString(),
            });
        }
        
//        private string Convert(EnumValueDesc field)
//        {
//            var jsonConverterAttr = field.EnumType.GetCustomAttribute<JsonConverterAttribute>();
//            if (jsonConverterAttr == null) return field.Value.ToString();
//            
//            var json = JsonConvert.SerializeObject(field.Value, field.EnumType, new JsonSerializerSettings
//            {
//                Converters = { (JsonConverter)Activator.CreateInstance(jsonConverterAttr.ConverterType) },
//            });
//            var str = JToken.Parse(json).Value<string>();
//            return str;
//        }
    }
}