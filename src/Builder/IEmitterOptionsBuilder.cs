namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptionsBuilder<out TOptions> where TOptions: IEmitterOptions
    {
        void SetFileTemplate();
        void SetIdentSize();
        void SetOneContractWrapper();
        void SetManyContractsWrapper();
        void SetFieldStringify();
        void SetContractStringify();
        void SetParser();

        TOptions Build();
    }
}