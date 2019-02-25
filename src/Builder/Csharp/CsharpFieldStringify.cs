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
{{~ if example ~}}
/// <example>{{ example }}</example>
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
                fieldDesc.Example,
                Name = _options.FieldNamingStrategy.GetPropertyName(fieldDesc.Name, false),
                fieldDesc.DeprecationCause,
                JsonProperty = fieldDesc.JsonName,
                IsNullable = fieldDesc.Type is NullableDesc,
                PropType = FieldTypeStringify(fieldDesc.Type),
                Validations = fieldDesc.Validation.Select(ValidationStringify)
            });
        }
        
        public string FieldTypeStringify(ITypeDesc type, bool isNullable = false)
        {
            switch (type)
            {
                case NullableDesc t:
                    return FieldTypeStringify(t.ElementType, true);
                case ContractDesc t:
                    return _options.ContractNameTempate(t);
                case ConstructedGenericDesc t:
                    return $"{_options.ContractNameTempate(t)}<{GenericArgsStringify(t.Arguments)}>";
                case GenericParametrDesc t:
                    return t.Name;
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
                            ptype = nullableType("bool");
                            break;
                        case PrimitiveDesc.Primitives.INT:
                            ptype = nullableType("int");
                            break;
                        case PrimitiveDesc.Primitives.LONG:
                            ptype = nullableType("long");
                            break;
                        case PrimitiveDesc.Primitives.DECIMAL:
                            ptype = nullableType("decimal");
                            break;
                        case PrimitiveDesc.Primitives.FLOAT:
                            ptype = nullableType("float");
                            break;
                        case PrimitiveDesc.Primitives.USHORT:
                            ptype = nullableType("ushort");
                            break;
                        case PrimitiveDesc.Primitives.STRING:
                            ptype = "string";
                            break;
                        case PrimitiveDesc.Primitives.DATETIME:
                            ptype = nullableType("DateTime");
                            break;
                        case PrimitiveDesc.Primitives.BYTE:
                            ptype = nullableType("byte");
                            break;
                        default:
                            throw new BuilderException("unknow primitive type");
                    }

                    return ptype;
                default:
                    return typeof(object).Name;
            }

            string nullableType(string ptype)
            {
                return isNullable ? $"{ptype}?" : ptype;
            }
        }

        private string GenericArgsStringify(IEnumerable<ITypeDesc> args)
        {
            return string.Join(", ", args.Select((t) => FieldTypeStringify(t)));
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