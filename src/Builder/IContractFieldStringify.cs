using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder
{
    public interface IContractFieldStringify
    {
        string Stringify(ContractFieldDesc fieldDesc);
    }
}