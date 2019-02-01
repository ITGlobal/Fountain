using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder
{
    public interface IManyContractsWrapper: ICodeWrapper
    {
        string WrapAll(string str, ContractGroup group);
        string WrapOne(string str);
    }
}