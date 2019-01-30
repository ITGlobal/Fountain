using System;

namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEmitterOptionsBuilder : EmitterOptionsBuilder
    {
        public TypescriptEmitterOptionsBuilder(Action<EmitterOptionsBuilder, IEmitterOptions> setup) : base(setup)
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
    }
}
