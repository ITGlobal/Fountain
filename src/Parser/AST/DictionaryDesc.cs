namespace ITGlobal.Fountain.Parser
{
    public class DictionaryDesc: ITypeDesc
    {
        public ITypeDesc KeyType { get; set; }
        public ITypeDesc ValueType { get; set; }
        public string Name { get; set; } = "dictionary";
    }
}