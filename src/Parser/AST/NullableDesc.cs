namespace ITGlobal.Fountain.Parser
{
    public class NullableDesc: ITypeDesc
    {
        public ITypeDesc ElementType { get; set; }
        public string Name { get; } = "nullable";
    }
}