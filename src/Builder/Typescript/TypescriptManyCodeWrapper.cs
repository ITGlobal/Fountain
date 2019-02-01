using System;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptManyCodeWrapper : IManyContractsWrapper
    {
        private readonly TypescriptEmitterOptions _options;

        public TypescriptManyCodeWrapper(TypescriptEmitterOptions options)
        {
            _options = options;
        }

        public string WrapAll(string str, ContractGroup group)
        {
            // calculate all base classes and write union types to end of namespace 
            var bases = string.Join(
                Environment.NewLine + Environment.NewLine,
                group.Groups.Values.ToArray()
                    .SelectMany(_ => _)
                    .Where(_ => (_ is ContractDesc cd) && cd.Base != null && !string.IsNullOrWhiteSpace(cd.Base.Name))
                    .GroupBy(_ => (_ as ContractDesc).Base.Name)
                    .Select(_ => $@"export type I{_options.ContractNameTempate(new ContractDesc {Name = _.Key})} =
{string.Join(Environment.NewLine, _.ToArray().Select(nestType => Utils.Ident($"| I{_options.ContractNameTempate(nestType)}", _options.IdentSize)))}"));

            return $@"
{NamespaceOrModule} {{
{Utils.Ident(str, _options.IdentSize)}

{Utils.Ident(bases, _options.IdentSize)}
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