using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder
{
    [PublicAPI]
    public interface IContractStringify
    {
        string Stringify(ContractDesc contractDesc);
    }
}