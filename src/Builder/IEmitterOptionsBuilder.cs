namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptionsBuilder<out TOptions> where TOptions: IEmitterOptions
    {
        void SetOneContractWrapper();
        void SetManyContractsWrapper();
        void SetFieldStringify();
        void SetContractStringify();
        void SetParserOptions();
        void SetParser();
        void SetContractEnumStringify();
        void SetEnumFieldStringify();
        

        TOptions Build();
    }
}