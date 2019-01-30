namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptCodeWrapper: ICodeWrapper
    {
        private readonly IEmitterOptions _options;

        public TypescriptCodeWrapper(IEmitterOptions options)
        {
            _options = options;
        }
        
        public string Wrap(string contractStr)
        {
            return $@"
declare module 'contracts' {{

}} 
";
        }
    }
}