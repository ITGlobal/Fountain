using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace ITGlobal.Fountain.Builder.Csharp
{
    public class CsharpEmitterOptionsBuilder: EmitterOptionsBuilder<CsharpEmitterOptions>
    {
        public CsharpEmitterOptionsBuilder([CanBeNull] Action<IEmitterOptionsBuilderSetup> setup = null): base(setup)
        {
        }
        
        public override void SetOneContractWrapper()
        {
            SetOneContractWrapper<CsharpOneContractWrapper>();
        }

        public override void SetManyContractsWrapper()
        {
            SetManyContractsWrapper<CsharpManyContractsWrapper>();
        }

        public override void SetFieldStringify()
        {
            SetFieldStringify<CsharpFieldStringify>();
        }

        public override void SetContractStringify()
        {
            SetContractStringify<CsharpContractStringify>();
        }

        public override void SetParserOptions()
        {
            SetParserOptions<ParserOptionsDefault>();
        }

        public override void SetParser()
        {
            SetParser<ParserAssebly>();
        }

        public override void SetContractEnumStringify()
        {
            SetContractEnumStringify<CsharpContractEnumStringify>();
        }

        public override void SetEnumFieldStringify()
        {
            SetEnumFieldStringify<CsharpEnumFieldStringify>();
        }

        public override CsharpEmitterOptions Build()
        {
            _serviceCollection.AddSingleton<CsharpTemplateContext>();
            return BuildBase(new CsharpEmitterOptions());
        }
    }
}