using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEmitterOptionsBuilder : EmitterOptionsBuilder<TypescriptEmitterOptions>
    {
        public TypescriptEmitterOptionsBuilder([CanBeNull]Action<IEmitterOptionsBuilderSetup> setup = null) : base(setup)
        {
        }

        public override void SetOneContractWrapper()
        {
            SetOneContractWrapper<TypescriptPerFileCodeWrapper>();
        }

        public override void SetManyContractsWrapper()
        {
            SetManyContractsWrapper<TypescriptManyCodeWrapper>();
        }

        public override void SetFieldStringify()
        {
            SetFieldStringify<TypescriptContractFieldStringify>();
        }

        public override void SetContractStringify()
        {
            SetContractStringify<TypescriptContractStringify>();
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
            SetContractEnumStringify<TypescriptContractEnumStringify>();
        }

        public override void SetEnumFieldStringify()
        {
            SetEnumFieldStringify<TypescriptEnumFieldStringify>();
        }

        public override TypescriptEmitterOptions Build()
        {
            _serviceCollection.AddSingleton<TypescriptJsDocComments>();
            return BuildBase(new TypescriptEmitterOptions());
        }
    }
}
