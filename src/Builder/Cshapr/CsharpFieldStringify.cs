using System;
using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpFieldStringify: IContractFieldStringify
    {
        private readonly CsharpEmitterOptions _options;
        private readonly Template _template;

        public CsharpFieldStringify(CsharpEmitterOptions options)
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
{{~ if json_property ~}}
[JsonProperty(""{{ json_property }}"")]
{{~ end ~}}
{{~ if is_nullable ~}}
[CanBeNull]
{{~ end ~}}
public {{prop_type}} {{name}} { get; set; }");
        }
        
        public string Stringify(ContractFieldDesc fieldDesc)
        {
            return _template.Render(new
            {
                fieldDesc.IsDeprecated,
                fieldDesc.Description,
                Name = _options.FieldNamingStrategy.GetPropertyName(fieldDesc.Name, false),
                fieldDesc.DeprecationCause,
                fieldDesc.JsonProperty,
                IsNullable = fieldDesc.Type is NullableDesc,
                PropType = FieldTypeStringify(fieldDesc.Type)
            });
        }
        
        private string FieldTypeStringify(ITypeDesc type, bool isNullable = false)
        {
            switch (type)
            {
                case NullableDesc t:
                    return FieldTypeStringify(t.ElementType, true);
                case ContractDesc t:
                    return _options.ContractNameTempate(t);
                case ContractEnumDesc t:
                    return _options.ContractNameTempate(t);
                case ArrayDesc t:
                    return $"{FieldTypeStringify(t.ElementType)}[]";
                case DictionaryDesc t:
                    return $"Dictionary<{FieldTypeStringify(t.KeyType)}, {FieldTypeStringify(t.ValueType)}>";
                case PrimitiveDesc t:
                    string ptype;
                    switch (t.Type)
                    {
                        case PrimitiveDesc.Primitives.BOOLEAN:
                            ptype = "bool";
                            break;
                        case PrimitiveDesc.Primitives.INT:
                            ptype = "int";
                            break;
                        case PrimitiveDesc.Primitives.LONG:
                            ptype = "long";
                            break;
                        case PrimitiveDesc.Primitives.DECIMAL:
                            ptype = "decimal";
                            break;
                        case PrimitiveDesc.Primitives.FLOAT:
                            ptype = "float";
                            break;
                        case PrimitiveDesc.Primitives.USHORT:
                            ptype = "ushort";
                            break;
                        case PrimitiveDesc.Primitives.STRING:
                            ptype = "string";
                            break;
                        case PrimitiveDesc.Primitives.DATETIME:
                            ptype = "DateTime";
                            break;
                        case PrimitiveDesc.Primitives.BYTE:
                            ptype = "byte";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return isNullable ? $"{ptype}?" : ptype;
                default:
                    return typeof(object).Name;
            }
        }
    }
}