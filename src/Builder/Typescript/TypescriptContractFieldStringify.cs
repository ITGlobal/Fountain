using System;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractFieldStringify : IContractFieldStringify
    {
        
        private readonly IEmitterOptions _options;
        private readonly TypescriptJsDocComments _jsDoc;

        public TypescriptContractFieldStringify(IEmitterOptions options, TypescriptJsDocComments jsDoc)
        {
            _options = options;
            _jsDoc = jsDoc;
        }

        public string Stringify(ContractFieldDesc fieldDesc)
        {
            return $@"{_jsDoc.Format(fieldDesc)}{FieldName(fieldDesc)}{MayBeMissing(fieldDesc)}: {FieldTypeStringify(fieldDesc.Type)}";
        }

        private string FieldName(ContractFieldDesc fieldDesc)
        {
            return fieldDesc.JsonProperty?.PropertyName ??
                   _options.FieldNamingStrategy.GetPropertyName(fieldDesc.Name, false);
        }

        private string MayBeMissing(ContractFieldDesc fieldDesc)
        {
            return fieldDesc.MayBeMissing ? "?" : "";
        }

        private string FieldTypeStringify(ITypeDesc type)
        {
            switch (type)
            {
                case NullableDesc t:
                    return $"{FieldTypeStringify(t.ElementType)} | null";
                case ContractDesc t:
                    return $"I{_options.ContractNameTempate(t)}";
                case ContractEnumDesc t:
                    return $"I{_options.ContractNameTempate(t)}";
                case ArrayDesc t:
                    return $"{FieldTypeStringify(t.ElementType)}[]";
                case DictionaryDesc t:
                    return $"{{ [key: string]: {FieldTypeStringify(t.ValueType)} }}";
                case PrimitiveDesc t:
                    switch (t.Type)
                    {
                        case PrimitiveDesc.Primitives.BOOLEAN:
                            return "boolean";
                        case PrimitiveDesc.Primitives.INT:
                        case PrimitiveDesc.Primitives.LONG:
                        case PrimitiveDesc.Primitives.DECIMAL:
                        case PrimitiveDesc.Primitives.FLOAT:
                        case PrimitiveDesc.Primitives.USHORT:
                            return "number";
                        case PrimitiveDesc.Primitives.STRING:
                        case PrimitiveDesc.Primitives.DATETIME:
                        case PrimitiveDesc.Primitives.BYTE:
                            return "string";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    return "any";
            }
        }
    }
}