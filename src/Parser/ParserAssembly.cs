using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ITGlobal.Fountain.Annotations;
using ITGlobal.Fountain.Annotations.Validation;
using ITGlobal.Fountain.Parser.Validation;
using JetBrains.Annotations;
using Newtonsoft.Json;

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

        private readonly ConcurrentDictionary<string, ITypeDesc> _contractCache
            = new ConcurrentDictionary<string, ITypeDesc>();
        
        private readonly ConcurrentDictionary<string, ContractEnumDesc> _enumCache
            = new ConcurrentDictionary<string, ContractEnumDesc>();
        
        private readonly ConcurrentDictionary<string, ConstructedGenericDesc> _constructedGenericCache
            = new ConcurrentDictionary<string, ConstructedGenericDesc>();
        
        private readonly ConcurrentDictionary<string, GenericDesc> _genericCache
            = new ConcurrentDictionary<string, GenericDesc>();
        
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
                        // абстрактные классы пропускаем т.к. их поля будут включены в результат
                        .Where(_ => !_.IsAbstract && 
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
            return _contractCache.GetOrAdd(t.FullName, s =>
            {
                var attrs = t.GetCustomAttributes().Where(_ => _ is IBaseAttribute);
                var docAttr = attrs.FirstOrDefault(_ => _ is DocumentationAttribute) as DocumentationAttribute;
                var deprecatedAttribute = attrs.FirstOrDefault(_ => _ is DeprecatedAttribute) as DeprecatedAttribute;
                var contractAttribute = attrs.FirstOrDefault(_ => _ is ContractAttribute) as ContractAttribute;
                
                return new ContractDesc
                {
                    Name = t.Name,
                    Description = docAttr?.Text,
                    Fields = ParseContractFields(t),
                    Base = ParseContractBase(t),
                    IsDeprecated = deprecatedAttribute != null,
                    DeprecationCause = deprecatedAttribute?.Cause,
                    Metadata = attrs.ToDictionary(_ => _.GetType().Name, _ => _),
                    CanBePartial = contractAttribute?.CanBePartial ?? false,
                    CustomAttributes = t.GetCustomAttributes<EmitAttributeAttribute>().Select(_ => new CustomAttibuteDesc
                    {
                        AttributeStr = _.AttributeStr,
                        Destination = _.Destination,
                    }).Where(_ => _options.CustomAttrDestinationFilter(_.Destination)),
                };
            });
        }
        
        private ConstructedGenericDesc ConstructedGeneric(Type t)
        {
            return _constructedGenericCache.GetOrAdd(t.FullName, s =>
            {
                return new ConstructedGenericDesc()
                {
                    Name = t.Name.Split('`')[0],
                    Arguments = t.GetGenericArguments().Select(ParseTypeDesc)
                };
            });
        }
        
        private GenericDesc Generic(Type t)
        {
            return _genericCache.GetOrAdd(t.FullName, s =>
            {
                var attrs = t.GetCustomAttributes().Where(_ => _ is IBaseAttribute);
                var docAttr = attrs.FirstOrDefault(_ => _ is DocumentationAttribute) as DocumentationAttribute;
                var deprecatedAttribute = attrs.FirstOrDefault(_ => _ is DeprecatedAttribute) as DeprecatedAttribute;
                var contractAttribute = attrs.FirstOrDefault(_ => _ is ContractAttribute) as ContractAttribute;
            
                return new GenericDesc
                {
                    Name = t.Name.Split('`')[0],
                    IsDeprecated = deprecatedAttribute != null,
                    DeprecationCause = deprecatedAttribute?.Cause,
                    Description = docAttr?.Text,
                    CanBePartial = contractAttribute?.CanBePartial ?? false,
                    Arguments = t.GetGenericArguments().Select(GenericParametr),
                    Metadata = attrs.ToDictionary(_ => _.GetType().Name, _ => _),
                    Fields = ParseContractFields(t),
                };
            });
        }

        private GenericParametrDesc GenericParametr(Type t)
        {
            return new GenericParametrDesc()
            {
                Name = t.Name,
            };
        }


        public virtual ITypeDesc Primitive(PrimitiveDesc.Primitives type)
        {
            return _primitiveTypesCache.GetOrAdd(PrimitiveDesc.GetName(type), key => new PrimitiveDesc(type));
        }

        public virtual ITypeDesc Enum(Type t)
        {
            return _enumCache.GetOrAdd(t.FullName, s =>
            {
                var documentation = t.GetCustomAttribute<DocumentationAttribute>(inherit: false)?.Text;
                var typeName = t.GetCustomAttribute<TypeNameAttribute>(inherit: false)?.Name ?? t.Name;
                var deprecatedEnumAttribute = t.GetCustomAttribute<DeprecatedAttribute>(inherit: false);
                var jsonConverter = t.GetCustomAttribute<JsonConverterAttribute>();
                
                return new ContractEnumDesc
                {
                    Name = typeName,
                    IsDeprecated = deprecatedEnumAttribute != null,
                    DeprecationCause = deprecatedEnumAttribute?.Cause,
                    Description = documentation,
                    JsonConverterType = jsonConverter?.ConverterType,
                    Values = EnumValueIterator().ToArray(),
                };
            });

            IEnumerable<EnumValueDesc> EnumValueIterator()
            {
                foreach (var value in System.Enum.GetValues(t))
                {
                    var member = t.GetMember(value.ToString()).FirstOrDefault();
                    if (member == null)
                    {
                        continue;
                    }

                    var description = member.GetCustomAttribute<DocumentationAttribute>(inherit: false)?.Text;
                    var jsonName = member.GetCustomAttribute<JsonNameAttribute>(inherit: false)?.Name;
                    var deprecatedAttribute = member.GetCustomAttribute<DeprecatedAttribute>(inherit: false);
 
                    yield return new EnumValueDesc
                    {
                        Description = description,
                        IsDeprecated = deprecatedAttribute != null,
                        DeprecationCause = deprecatedAttribute?.Cause,
                        EnumType = t,
                        MemberInfo = member,
                        JsonName = jsonName,
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
        
        [CanBeNull]
        public virtual ITypeDesc ParseContractBase(Type t)
        {
            if (t.BaseType == null)
            {
                return null;
            }

            if (t.BaseType.IsAbstract)
            {
                return null;
            }
            
            if (t.BaseType.GetCustomAttribute<ContractAttribute>() == null)
            {
                return null;
            }
            
            return ParseTypeDesc(t.BaseType);
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
            var jsonName = property.GetCustomAttribute<JsonNameAttribute>();
            var queryName = property.GetCustomAttribute<QueryAttribute>();
            var mayBeMissingAttribute = property.GetCustomAttribute<MayBeMissingAttribute>();
            var canBeNull = property.GetCustomAttribute<NullableAttribute>() != null;
            var typeDesc = ParseTypeDesc(property.PropertyType);
            
            return new ContractFieldDesc
            {
                Name = property.Name,
                IsDeprecated = deprecation != null,
                DeprecationCause = deprecation?.Cause,
                MayBeMissing = mayBeMissingAttribute != null,
                Description = description?.Text,
                Example = description?.Example,
                JsonName = jsonName?.Name,
                QueryName = queryName?.Name,
                // if property marked by CanBeNull attribute, but property type isn't nullable, create NullableDesc
                Type = canBeNull && !(typeDesc is NullableDesc) ? new NullableDesc { ElementType = typeDesc } : typeDesc,
                Validation = ParseContractOneFieldValidation(property)
                    .Where(_ => _options.ValidationDestinationFilter(_.Destination)),
                CustomAttributes = property.GetCustomAttributes<EmitAttributeAttribute>().Select(_ => new CustomAttibuteDesc
                {
                    AttributeStr = _.AttributeStr,
                    Destination = _.Destination,
                }).Where(_ => _options.CustomAttrDestinationFilter(_.Destination)),
            };
        }

        public virtual IEnumerable<IFieldValidation> ParseContractOneFieldValidation(PropertyInfo property)
        {
            var requiredAttr = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                yield return new RequiredValidation { Destination = requiredAttr.Destination };
            }
            
            var emailAttr = property.GetCustomAttribute<EmailAttribute>();
            if (emailAttr != null)
            {
                yield return new EmailValidation { Destination = emailAttr.Destination };
            }
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

                return ConstructedGeneric(t);
            }

            if (t.IsGenericParameter)
            {
                return GenericParametr(t);
            }
            
            var contractAttr = t.GetCustomAttribute<ContractAttribute>();
            if (contractAttr == null)
            {
                return new AnyDesc();
            }
            
            if (t.IsGenericType)
            {
                return Generic(t);
            }

            if (t.IsEnum)
            {
                return Enum(t);
            }
            
            return Contract(t);
        }


        private bool IsNullable(Type t)
        {
            var canBeNullAttribute = t.GetCustomAttribute<NullableAttribute>();
            return canBeNullAttribute != null || Nullable.GetUnderlyingType(t) != null;
        }
    }
}