using System;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptManyCodeWrapper: IManyContractsWrapper
    {
        private readonly IEmitterOptions _options;

        public TypescriptManyCodeWrapper(IEmitterOptions options)
        {
            _options = options;
        }
        
        public string WrapAll(string str)
        {
            return $@"
declare module 'contracts' {{
{Utils.Ident(str, _options.IdentSize)}
}} 
";
        }

        public string WrapOne(string str)
        {
            return str;
        }
    }
}