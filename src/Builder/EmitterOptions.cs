using System;
using System.Collections.Generic;
using ITGlobal.Fountain.Builder.Exceptions;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    public class EmitterOptions : IEmitterOptions
    {
        public int IdentSize { get; set; }
        public string Filename { get; set; }
        public Func<string, ITypeDesc, string> FileTemplate { get; set; }
        public IPerFileContractWrapper PerFileContractWrapper { get; set; }
        public IManyContractsWrapper ManyContractsWrapper { get; set; }
        public IContractFieldStringify FieldStringify { get; set; }
        public IContractStringify ContractStringify { get; set; }
        public IContractEnumStringify ContractEnumStringify { get; set; }
        public IContractGenericStringify ContractGenericStringify { get; set; }
        public IEnumFieldStringify EnumFieldStringify { get; set; }
        public IParserOptions ParserOptions { get; set; }
        public IParserAssembly Parser { get; set; }
        public NamingStrategy FieldNamingStrategy { get; set; }
        public Func<ITypeDesc, string> ContractNameTempate { get; set; }

        public virtual void CheckOptions()
        {
            if (Filename == null && FileTemplate == null)
            {
                throw new CheckOptionsException(new[] {nameof(Filename), nameof(FileTemplate)},
                    "one of these fields must be defined");
            }
        }
    }
}