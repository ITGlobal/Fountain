using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpOneContractWrapper : IPerFileContractWrapper
    {
        private readonly CsharpEmitterOptions _options;
        private readonly CsharpTemplateContext _contextMaker;
        private readonly Template _template;

        public CsharpOneContractWrapper(CsharpEmitterOptions options, CsharpTemplateContext contextMaker)
        {
            _options = options;
            _contextMaker = contextMaker;
            _template = Template.Parse(
@"using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace {{ namespace }} {
{{ contract | ident }}
}");
        }

        public string Wrap(string str, string group, ITypeDesc contract)
        {
            return _template.Render(
                _contextMaker.Make(new
                {
                    Namespace = _options.CsharpNamespaceTemplatePerFile(group, contract),
                    Contract = str
                })
            );
        }
    }
}