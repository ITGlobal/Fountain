using System;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptContractFieldStringify : IContractFieldStringify
    {
        
        private readonly IEmitterOptions _options;

        public TypescriptContractFieldStringify(IEmitterOptions options)
        {
            _options = options;
        }

        public string Stringify(ContractFieldDesc fieldDesc)
        {
            return $@"{Description(fieldDesc)}{fieldDesc.JsonProperty?.PropertyName ?? _options.FieldNamingStrategy.GetPropertyName(fieldDesc.Name, false)}: {FieldTypeStringify(fieldDesc.Type)}";
        }

        private string FieldTypeStringify(ITypeDesc type)
        {
            switch (type)
            {
                case ContractDesc t:
                    return $"I{t.Name}";
                case PrimitiveTypeDesc t:
                    switch (t.Type)
                    {
                        case PrimitiveTypeDesc.Primitives.STRING:
                            return "string";
                        case PrimitiveTypeDesc.Primitives.BOOLEAN:
                            return "boolean";
                        case PrimitiveTypeDesc.Primitives.INT:
                            return "number";
                        case PrimitiveTypeDesc.Primitives.LONG:
                            return "number";
                        case PrimitiveTypeDesc.Primitives.DECIMAL:
                            return "number";
                        case PrimitiveTypeDesc.Primitives.DATETIME:
                            return "string";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    return "any";
            }
        }
        
        private string Description(ContractFieldDesc fieldDesc) => string.IsNullOrWhiteSpace(fieldDesc.Description)
            ? ""
            : $@"// {fieldDesc.Description}{Environment.NewLine}";
    }
}