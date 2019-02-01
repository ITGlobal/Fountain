namespace ITGlobal.Fountain.Parser
{
    public class ArrayDesc: ITypeDesc
    {
        public string Name { get; } = "array";
        public ITypeDesc ElementType { get; set; }
    }
}