namespace ITGlobal.Fountain.Builder
{
    public interface IManyContractsWrapper: ICodeWrapper
    {
        string WrapAll(string str);
        string WrapOne(string str);
    }
}