using System;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptPerFileCodeWrapper: IPerFileContractWrapper
    {
        private readonly IEmitterOptions _options;

        public TypescriptPerFileCodeWrapper(IEmitterOptions options)
        {
            _options = options;
        }
        
        public string Wrap(string str)
        {
            return $@"
declare module 'contracts' {{
{Utils.Ident(str, _options.IdentSize)}
}} 
";
        }
    }
}