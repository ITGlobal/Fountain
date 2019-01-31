using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ITGlobal.Fountain.Annotations;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Parser
{
    [PublicAPI]
    public class ParseAssebly: IParseAssembly
    {
        #region cache

        private readonly ConcurrentDictionary<string, PrimitiveTypeDesc> PrimitiveTypesCache
            = new ConcurrentDictionary<string, PrimitiveTypeDesc>();

        #endregion
        
        public virtual ContractGroup Parse(Assembly assembly)
        {
            return Parse(assembly.GetExportedTypes().Where(_ => _.GetCustomAttribute<ContractAttribute>() != null));
        }
        
        public virtual ContractGroup Parse(IEnumerable<Type> types)
        {
            return new ContractGroup
            {
                Groups =
                    types
                        
                        .GroupBy(_ => _.GetCustomAttribute<ContractAttribute>().Group).ToDictionary(
                        _ => _.Key,
                        _ => _.ToArray().Select(ParseTypeDesc)
                    )

            };
        }

        #region parse concrete type

                
        public virtual ITypeDesc Contract(Type t)
        {
            var attrs = t.GetCustomAttributes().Where(_ => _.GetType().IsAssignableFrom(typeof(IBaseAttribute)));
            var docAttr = attrs.FirstOrDefault(_ => _ is DocumentationAttribute) as DocumentationAttribute;
            return new ContractDesc
            {
                Name = t.Name,
                Description = docAttr?.Text,
                IsGeneric = t.IsGenericType,
                IsAbstract = t.IsAbstract,
                Fields = ParseContractFields(t),
                Generics = ParseContractGenerics(t),
                Base = ParseContractBase(t),
                Metadata = attrs.ToDictionary(_ => _.GetType().Name, _ => _)
            };
        }
        
        public virtual ITypeDesc Primitive(PrimitiveTypeDesc.Primitives type)
        {
            return PrimitiveTypesCache.GetOrAdd(getName(), key => new PrimitiveTypeDesc { Name = key, Type = type});
            
            string getName() {
                switch (type)
                {
                    case PrimitiveTypeDesc.Primitives.STRING:
                        return "string";
                    case PrimitiveTypeDesc.Primitives.BOOLEAN:
                        return "bool";
                    case PrimitiveTypeDesc.Primitives.INT:
                        return "int";
                    case PrimitiveTypeDesc.Primitives.LONG:
                        return "long";
                    case PrimitiveTypeDesc.Primitives.DECIMAL:
                        return "decimal";
                    case PrimitiveTypeDesc.Primitives.DATETIME:
                        return "datetime";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        #endregion

        
        public virtual ContractDesc ParseContractBase(Type t)
        {
            return new ContractDesc();
        }

        public virtual IEnumerable<ContractEnumDesc> ParseContractEnums(Type t)
        {
            return new List<ContractEnumDesc>();
        }

        public virtual IEnumerable<ContractGenericDesc> ParseContractGenerics(Type t)
        {
            return new List<ContractGenericDesc>();
        }

        public virtual IEnumerable<ContractFieldDesc> ParseContractFields(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(_ => _.GetCustomAttribute<ContractFieldAttribute>() != null)
                .Select(ParseContractOneField);
        }

        public virtual ContractFieldDesc ParseContractOneField(PropertyInfo property)
        {
            var deprecation = property.GetCustomAttribute<DeprecatedAttribute>();
            var description = property.GetCustomAttribute<DocumentationAttribute>();
            var jsonAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
            return new ContractFieldDesc
            {
                Name = property.Name,
                IsDeprecated = deprecation != null,
                DeprecationCause = deprecation?.Cause,
                Description = description?.Text,
                JsonProperty = jsonAttribute,
                Type = ParseTypeDesc(property.PropertyType)
            };
        }

        public virtual ITypeDesc ParseTypeDesc(Type t)
        {
            if (t == typeof(bool))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.BOOLEAN);
            }
            if (t == typeof(string))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.STRING);
            }
            if (t == typeof(int))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.INT);
            }
            if (t == typeof(long))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.LONG);
            }
            if (t == typeof(decimal))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.DECIMAL);
            }
            if (t == typeof(DateTime))
            {
                return Primitive(PrimitiveTypeDesc.Primitives.DATETIME);
            }

            var contractAttr = t.GetCustomAttribute<ContractAttribute>();
            if (contractAttr == null)
            {
                return new AnyTypeDesc();
            }
            
            return Contract(t);
        }



    }
}