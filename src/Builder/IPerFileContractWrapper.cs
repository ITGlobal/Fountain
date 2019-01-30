namespace ITGlobal.Fountain.Builder
{
    public interface IPerFileContractWrapper: ICodeWrapper
    {
        string Wrap(string str);
    }
}