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
    public class ParserAssebly: IParserAssembly
    {
        private readonly IParserOptions _options;

        public ParserAssebly(IParserOptions options)
        {
            _options = options;
        }

        #region cache

        private readonly ConcurrentDictionary<string, PrimitiveDesc> _primitiveTypesCache
            = new ConcurrentDictionary<string, PrimitiveDesc>();

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
                        // генерики и абстрактные классы пропускаем т.к. их поля будут включены в результат
                        .Where(_ => !_.IsGenericType && 
                                    !_.IsAbstract && 
                                    !_.IsInterface &&
                                    // фильтруем контракты
                                    _options.FilterContracts(_.GetCustomAttribute<ContractAttribute>()))
                        .GroupBy(_ => _.GetCustomAttribute<ContractAttribute>().Group).ToDictionary(
                        _ => _.Key,
                        _ => _.ToArray().Select(ParseTypeDesc)
                    )

            };
        }

        #region parse concrete type

                
        public virtual ITypeDesc Contract(Type t)
        {
            var attrs = t.GetCustomAttributes().Where(_ => _ is IBaseAttribute);
            var docAttr = attrs.FirstOrDefault(_ => _ is DocumentationAttribute) as DocumentationAttribute;
            var deprecatedAttribute = attrs.FirstOrDefault(_ => _ is DeprecatedAttribute) as DeprecatedAttribute;
            return new ContractDesc
            {
                Name = t.Name,
                Description = docAttr?.Text,
                Fields = ParseContractFields(t),
                Base = ParseContractBase(t),
                IsDeprecated = deprecatedAttribute != null,
                DeprecationCause = deprecatedAttribute?.Cause ?? string.Empty,
                Metadata = attrs.ToDictionary(_ => _.GetType().Name, _ => _)
            };
        }
        
        public virtual ITypeDesc Primitive(PrimitiveDesc.Primitives type)
        {
            return _primitiveTypesCache.GetOrAdd(PrimitiveDesc.GetName(type), key => new PrimitiveDesc(type));
        }

        public virtual ITypeDesc Enum(Type t)
        {
            var documentation = t.GetCustomAttribute<DocumentationAttribute>(inherit: false)?.Text ?? string.Empty;
            var typeName = t.GetCustomAttribute<TypeNameAttribute>(inherit: false)?.Name ?? t.Name;
            var deprecatedEnumAttribute = t.GetCustomAttribute<DeprecatedAttribute>(inherit: false);

            return new ContractEnumDesc
            {
                Name = typeName,
                IsDeprecated = deprecatedEnumAttribute != null,
                DeprecationCause = deprecatedEnumAttribute?.Cause ?? string.Empty,
                Description = documentation,
                Values = EnumValueIterator().ToArray(),
            };
            
            IEnumerable<EnumValueDesc> EnumValueIterator()
            {
                foreach (var value in System.Enum.GetValues(t))
                {
                    var member = t.GetMember(value.ToString()).FirstOrDefault();
                    if (member == null)
                    {
                        continue;
                    }

                    var description = member.GetCustomAttribute<DocumentationAttribute>(inherit: false)?.Text ?? string.Empty;
                    var deprecatedAttribute = member.GetCustomAttribute<DeprecatedAttribute>(inherit: false);
 
                    yield return new EnumValueDesc
                    {
                        Description = description,
                        IsDeprecated = deprecatedAttribute != null,
                        DeprecationCause = deprecatedAttribute?.Cause ?? string.Empty,
                        EnumType = t,
                        MemberInfo = member,
                        Value = value,
                    };
                }
            }
        }

        public virtual ArrayDesc Array(Type t)
        {
            return new ArrayDesc
            {
                ElementType = ParseTypeDesc(t.GetElementType())
            }; 
        }

        public virtual DictionaryDesc Dictionary(Type t)
        {
            var keyType = t.GetGenericArguments()[0];
            var valueType = t.GetGenericArguments()[1];
            return new DictionaryDesc
            {
                KeyType = ParseTypeDesc(keyType),
                ValueType = ParseTypeDesc(valueType),
            };
        }

        public virtual NullableDesc Null(Type t)
        {
            return new NullableDesc
            {
                ElementType = ParseTypeDesc(Nullable.GetUnderlyingType(t) ?? t),
            };
        }

        #endregion
   
        public virtual ContractDesc ParseContractBase(Type t)
        {
            return new ContractDesc();
        }

        public virtual IEnumerable<ContractFieldDesc> ParseContractFields(Type t)
        {
            var selfTypeFields = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(_ =>
                {
                    var attr = _.GetCustomAttribute<ContractFieldAttribute>();
                    return attr != null && _options.FilterFields(attr);
                })
                .Select(ParseContractOneField);
            return selfTypeFields;
        }

        public virtual ContractFieldDesc ParseContractOneField(PropertyInfo property)
        {
            var deprecation = property.GetCustomAttribute<DeprecatedAttribute>();
            var description = property.GetCustomAttribute<DocumentationAttribute>();
            var jsonAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
            var mayBeMissingAttribute = property.GetCustomAttribute<MayBeMissingAttribute>();
            var canBeNull = property.GetCustomAttribute<CanBeNullAttribute>() != null;
            var typeDesc = ParseTypeDesc(property.PropertyType);
            return new ContractFieldDesc
            {
                Name = property.Name,
                IsDeprecated = deprecation != null,
                DeprecationCause = deprecation?.Cause ?? string.Empty,
                MayBeMissing = mayBeMissingAttribute != null,
                Description = description?.Text,
                JsonProperty = jsonAttribute,
                // if property marked by CanBeNull attribute, but property type isn't nullable, create NullableDesc
                Type = canBeNull && !(typeDesc is NullableDesc) ? new NullableDesc { ElementType = typeDesc } : typeDesc, 
            };
        }

        public virtual ITypeDesc ParseTypeDesc(Type t)
        {
            if (IsNullable(t))
            {
                return Null(t);
            }
            if (t == typeof(bool))
            {
                return Primitive(PrimitiveDesc.Primitives.BOOLEAN);
            }
            if (t == typeof(string))
            {
                return Primitive(PrimitiveDesc.Primitives.STRING);
            }
            if (t == typeof(int))
            {
                return Primitive(PrimitiveDesc.Primitives.INT);
            }
            if (t == typeof(long))
            {
                return Primitive(PrimitiveDesc.Primitives.LONG);
            }
            if (t == typeof(decimal))
            {
                return Primitive(PrimitiveDesc.Primitives.DECIMAL);
            }
            if (t == typeof(float))
            {
                return Primitive(PrimitiveDesc.Primitives.FLOAT);
            }
            if (t == typeof(ushort))
            {
                return Primitive(PrimitiveDesc.Primitives.USHORT);
            }
            if (t == typeof(DateTime))
            {
                return Primitive(PrimitiveDesc.Primitives.DATETIME);
            }
            if (t == typeof(byte))
            {
                return Primitive(PrimitiveDesc.Primitives.BYTE);
            }

            if (t.IsArray)
            {
                return Array(t);
            }

            if (t.IsConstructedGenericType)
            {
                if (t.Name.Contains("Dictionary"))
                {
                    return Dictionary(t);
                }
            }

            var contractAttr = t.GetCustomAttribute<ContractAttribute>();
            if (contractAttr == null)
            {
                return new AnyDesc();
            }

            if (t.IsEnum)
            {
                return Enum(t);
            }
            
            return Contract(t);
        }

        private bool IsNullable(Type t)
        {
            var canBeNullAttribute = t.GetCustomAttribute<CanBeNullAttribute>();
            return canBeNullAttribute != null || Nullable.GetUnderlyingType(t) != null;
        }
    }
}