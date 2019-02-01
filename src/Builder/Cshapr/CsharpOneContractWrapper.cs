using System;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpOneContractWrapper: IPerFileContractWrapper
    {
        private readonly CsharpEmitterOptions _options;
        
        public CsharpOneContractWrapper(CsharpEmitterOptions options)
        {
            _options = options;
        }
        
        public string Wrap(string str, string group, ITypeDesc contract)
        {
            return $"namespace {_options.CsharpNamespaceTemplate(group, contract)} {{{Environment.NewLine}" +
                   $"{Utils.Ident(str, _options.IdentSize)}{Environment.NewLine}" +
                   $"}}";
        }
    }
}