using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEnumFieldStringify: IEnumFieldStringify
    {
        private readonly TypescriptJsDocComments _jsDoc;

        public TypescriptEnumFieldStringify(TypescriptJsDocComments jsDoc)
        {
            _jsDoc = jsDoc;
        }
        public string Stringify(EnumValueDesc field)
        {
            return $@"{_jsDoc.Format(field)}| '{Convert(field)}'";
        }

        private string Convert(EnumValueDesc field)
        {
            return field.JsonName ?? field.Value.ToString();
//            var jsonConverterAttr = field.EnumType.GetCustomAttribute<JsonConverterAttribute>();
//            if (jsonConverterAttr == null) return field.Value.ToString();
//            
//            var json = JsonConvert.SerializeObject(field.Value, field.EnumType, new JsonSerializerSettings
//            {
//                Converters = { (JsonConverter)Activator.CreateInstance(jsonConverterAttr.ConverterType) },
//            });
//            var str = JToken.Parse(json).Value<string>();
//            return str;
        }
    }
}