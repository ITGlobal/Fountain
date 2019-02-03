using ITGlobal.Fountain.Parser;
using Scriban;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpManyContractsWrapper: IManyContractsWrapper
    {
        private readonly CsharpEmitterOptions _options;
        private readonly CsharpTemplateContext _contextMaker;
        private readonly Template _template;

        public CsharpManyContractsWrapper(CsharpEmitterOptions options, CsharpTemplateContext contextMaker)
        {
            _options = options;
            _contextMaker = contextMaker;
            _template = Template.Parse(
                @"using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace {{ namespace }} {
{{ contracts | ident }}
}");
        }
        public string WrapAll(string str, ContractGroup @group)
        {
            return _template.Render(
                _contextMaker.Make(new
                {
                    Namespace = _options.CsharpNamespaceOneFile,
                    Contracts = str
                })
            );
        }

        public string WrapOne(string str)
        {
            return str;
        }
    }
}