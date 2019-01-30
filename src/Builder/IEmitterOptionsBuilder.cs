namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptionsBuilder
    {
        void SetFileTemplate();
        void SetIdentSize();
        void SetOneContractWrapper();
        void SetManyContractsWrapper();
        void SetFieldStringify();
        void SetContractStringify();

        IEmitterOptions Build();
    }
}