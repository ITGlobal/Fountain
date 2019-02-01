using System;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptPerFileCodeWrapper : IPerFileContractWrapper
    {
        private readonly TypescriptEmitterOptions _options;

        public TypescriptPerFileCodeWrapper(TypescriptEmitterOptions options)
        {
            _options = options;
        }

        public string Wrap(string str, string group, ITypeDesc contract)
        {
            return $@"
{NamespaceOrModule} {{
{Utils.Ident(str, _options.IdentSize)}
}} 
";
        }

        private string NamespaceOrModule => _options.TypescriptModuleType == TypescriptModuleType.Namespace
            ? $"declare namespace {_options.Namespace}"
            : $"declare module '{_options.Module}'";
    }
}