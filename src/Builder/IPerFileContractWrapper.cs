using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder
{
    public interface IPerFileContractWrapper: ICodeWrapper
    {
        string Wrap(string str, string group, ITypeDesc contract);
    }
}