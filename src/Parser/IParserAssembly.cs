using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Parser
{
    [PublicAPI]
    public interface IParserAssembly
    {
        ContractGroup Parse(Assembly assembly);
        ContractGroup Parse(IEnumerable<Type> types);
        ITypeDesc Contract(Type t);
        ITypeDesc Primitive(PrimitiveDesc.Primitives type);
        ContractDesc ParseContractBase(Type t);
        IEnumerable<ContractFieldDesc> ParseContractFields(Type t);
        ContractFieldDesc ParseContractOneField(PropertyInfo property);
        ITypeDesc ParseTypeDesc(Type t);
    }
}