using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace Builder
{
    [PublicAPI]
    public interface IContractStringify
    {
        string Stringify(ContractDesc contractDesc);
    }
}