using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEmitterOptionsBuilder : EmitterOptionsBuilder<TypescriptEmitterOptions>
    {
        public TypescriptEmitterOptionsBuilder([CanBeNull]Action<EmitterOptionsBuilder<TypescriptEmitterOptions>> setup = null) : base(setup)
        {
        }

        public override void SetFileTemplate()
        {
            // нет настроек по умолчанию
        }

        public override void SetIdentSize()
        {
            // нет настроек по умолчанию
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

        public override void SetParser()
        {
            SetParser<ParseAssebly>();
        }

        public override TypescriptEmitterOptions Build()
        {
            return BuildBase(new TypescriptEmitterOptions(
            ));
        }
    }
}
