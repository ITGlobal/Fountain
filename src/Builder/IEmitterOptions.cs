using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptions
    {
        int IdentSize { get; set; }
        [CanBeNull] string Filename { get; set; }
        
        [CanBeNull] Func<string, ITypeDesc, string> FileTemplate { get; set; }
        [NotNull] IPerFileContractWrapper PerFileContractWrapper { get; set; }
        [NotNull] IManyContractsWrapper ManyContractsWrapper { get; set; }
        [NotNull] IContractFieldStringify FieldStringify { get; set; }
        [NotNull] IContractStringify ContractStringify { get; set; }
        [NotNull] IContractEnumStringify ContractEnumStringify { get; set; }
        [NotNull] IEnumFieldStringify EnumFieldStringify { get; set; }
        [NotNull] IParserOptions ParserOptions { get; set; }
        [NotNull] IParserAssembly Parser { get; set; }
        [NotNull] NamingStrategy FieldNamingStrategy { get; set; }
        [NotNull] Func<ITypeDesc, string> ContractNameTempate { get; set; }

        void CheckOptions();
    }
}