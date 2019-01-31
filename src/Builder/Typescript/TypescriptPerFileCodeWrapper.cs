using System;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptPerFileCodeWrapper : IPerFileContractWrapper
    {
        private readonly TypescriptEmitterOptions _options;

        public TypescriptPerFileCodeWrapper(TypescriptEmitterOptions options)
        {
            _options = options;
        }

        public string Wrap(string str)
        {
            return $@"
{NamespaceOrModule} {{
{Utils.Ident(str, _options.IdentSize)}
}} 
";
        }

        public string NamespaceOrModule => _options.TypescriptModuleType == TypescriptModuleType.Namespace
            ? $"declare namespace {_options.Namespace}"
            : $"declare module '{_options.Module}'";
    }
}