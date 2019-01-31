namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptManyCodeWrapper: IManyContractsWrapper
    {
        private readonly TypescriptEmitterOptions _options;

        public TypescriptManyCodeWrapper(TypescriptEmitterOptions options)
        {
            _options = options;
        }
        
        public string WrapAll(string str)
        {
            return $@"
{NamespaceOrModule} {{
{Utils.Ident(str, _options.IdentSize)}
}} 
";
        }

        public string WrapOne(string str)
        {
            return str;
        }
        
        private string NamespaceOrModule => _options.TypescriptModuleType == TypescriptModuleType.Namespace
            ? $"declare namespace {_options.Namespace}"
            : $"declare module '{_options.Module}'";
    }
}