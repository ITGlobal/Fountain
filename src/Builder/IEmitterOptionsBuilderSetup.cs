using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder
{
    public interface IEmitterOptionsBuilderSetup
    {
        void SetFieldStringify<T>() where T : class, IContractFieldStringify;
        void SetContractStringify<T>() where T : class, IContractStringify;
        void SetOneContractWrapper<T>() where T : class, IPerFileContractWrapper;
        void SetManyContractsWrapper<T>() where T : class, IManyContractsWrapper;
        void SetParserOptions<T>() where T : class, IParserOptions;
        void SetParser<T>() where T : class, IParserAssembly;
        void SetContractEnumStringify<T>() where T : class, IContractEnumStringify;
        void SetEnumFieldStringify<T>() where T : class, IEnumFieldStringify;
    }
}