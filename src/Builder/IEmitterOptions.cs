using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptions
    {
        int IdentSize { get; set; }
        [CanBeNull]
        string Filename { get; set; }
        [CanBeNull]
        Func<string, ITypeDesc, string> FileTemplate { get; set; }
        IPerFileContractWrapper PerFileContractWrapper { get; set; }
        IManyContractsWrapper ManyContractsWrapper { get; set; }
        IContractFieldStringify FieldStringify { get; set; }
        IContractStringify ContractStringify { get; set; }
        IContractEnumStringify ContractEnumStringify { get; set; }
        IEnumFieldStringify EnumFieldStringify { get; set; }
        IParseAssembly Parser { get; set; }
        NamingStrategy FieldNamingStrategy { get; set; }
        NamingStrategy ContractNamingStrategy { get; set; }

        void CheckOptions();
    }
}