using System;
using System.Collections.Generic;
using System.Linq;
using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptJsDocComments
    {
        public string Format<T>(T contract) where T : IDeprecatableDesc, IDescriptableDesc
        {
            var comments = new List<string> {Description(contract), Deprecate(contract)}
                .Where(_ => !string.IsNullOrEmpty(_)).ToArray();
            return !comments.Any()
                ? ""
                : $@"/**{Environment.NewLine}{string.Join(Environment.NewLine, comments)} */{Environment.NewLine}";

            string Description(IDescriptableDesc desc) => string.IsNullOrWhiteSpace(desc.Description)
                ? ""
                : $@" * {desc.Description}{Environment.NewLine}";

            string Deprecate(IDeprecatableDesc desc) => string.IsNullOrWhiteSpace(desc.DeprecationCause)
                ? ""
                : $@" * @deprecated {desc.DeprecationCause}{Environment.NewLine}";
        }
    }
}