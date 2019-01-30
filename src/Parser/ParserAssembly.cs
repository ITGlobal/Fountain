using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ITGlobal.Fountain.Annotations;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Parser
{
    [PublicAPI]
    public static class ParseAssebly
    {
        #region cache

        private static readonly ConcurrentDictionary<string, PrimitiveTypeDesc> PrimitiveTypesCache
            = new ConcurrentDictionary<string, PrimitiveTypeDesc>();

        #endregion
        
        public static ContractGroup Parse(Assembly assembly)
        {
            return Parse(assembly.GetExportedTypes().Where(_ => _.GetCustomAttribute<ContractAttribute>() != null));
        }
        
        public static ContractGroup Parse(IEnumerable<Type> types)
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

                
        public static ITypeDesc Contract(Type t)
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
                Bases = ParseContractBases(t),
                Metadata = attrs.ToDictionary(_ => _.GetType().Name, _ => _)
            };
        }
        
        private static ITypeDesc Primitive(string name)
        {
            return PrimitiveTypesCache.GetOrAdd(name, key => new PrimitiveTypeDesc { Key = key });
        }

        #endregion

        
        private static IEnumerable<ContractDesc> ParseContractBases(Type t)
        {
            return new List<ContractDesc>();
        }

        private static IEnumerable<ContractEnumDesc> ParseContractEnums(Type t)
        {
            return new List<ContractEnumDesc>();
        }

        private static IEnumerable<ContractGenericDesc> ParseContractGenerics(Type t)
        {
            return new List<ContractGenericDesc>();
        }

        private static IEnumerable<ContractFieldDesc> ParseContractFields(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(_ => _.GetCustomAttribute<ContractFieldAttribute>() != null)
                .Select(ParseContractOneField);
        }

        private static ContractFieldDesc ParseContractOneField(PropertyInfo property)
        {
            var deprecation = property.GetCustomAttribute<DeprecatedAttribute>();
            var description = property.GetCustomAttribute<DocumentationAttribute>();
            return new ContractFieldDesc
            {
                Name = property.Name,
                IsDeprecated = deprecation != null,
                DeprecationCause = deprecation?.Cause,
                Description = description?.Text,
                Type = ParseTypeDesc(property.PropertyType)
            };
        }

        private static ITypeDesc ParseTypeDesc(Type t)
        {
            if (t == typeof(bool))
            {
                return Primitive("bool");
            }
            if (t == typeof(string))
            {
                return Primitive("string");
            }
            if (t == typeof(int))
            {
                return Primitive("int");
            }
            if (t == typeof(long))
            {
                return Primitive("long");
            }
            if (t == typeof(decimal))
            {
                return Primitive("decimal");
            }
            if (t == typeof(DateTime))
            {
                return Primitive("datetime");
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