using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Builder.Exceptions;
using ITGlobal.Fountain.Parser;
using ITGlobal.Fountain.Parser.Validation;
using Scriban;

namespace ITGlobal.Fountain.Builder.Csharp
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
{{~ for v in validations ~}}
{{ v }}
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
                JsonProperty = fieldDesc.JsonName,
                IsNullable = fieldDesc.Type is NullableDesc,
                PropType = FieldTypeStringify(fieldDesc.Type),
                Validations = fieldDesc.Validation.Select(ValidationStringify)
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
                            throw new BuilderException("unknow primitive type");
                    }

                    return isNullable ? $"{ptype}?" : ptype;
                default:
                    return typeof(object).Name;
            }
        }

        private string ValidationStringify(IFieldValidation validation)
        {
            switch (validation)
            {
                case RequiredValidation v:
                    return "[Required]";
                case EmailValidation v:
                    return "[EmailAddress]";
                default:
                    throw new BuilderException("unknow validation type");
            }
        }
    }
}