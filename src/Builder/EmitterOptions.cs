using System;
using ITGlobal.Fountain.Parser;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    public class EmitterOptions: IEmitterOptions
    {
        public int IdentSize { get; set; }
        public string Filename { get; set; }
        public Func<string, ITypeDesc, string> FileTemplate { get; set; }
        public IPerFileContractWrapper PerFileContractWrapper { get; set; }
        public IManyContractsWrapper ManyContractsWrapper { get; set; }
        public IContractFieldStringify FieldStringify { get; set; }
        public IContractStringify ContractStringify { get; set; }
        public IParseAssembly Parser { get; set; }
        public NamingStrategy FieldNamingStrategy { get; set; }
        public NamingStrategy ContractNamingStrategy { get; set; }
        
        public virtual void CheckOptions()
        {
        }
    }
}