namespace ITGlobal.Fountain.Parser
{
    public sealed class PrimitiveTypeDesc: ITypeDesc
    {
        public string Name { get; set; }
        public Primitives Type { get; set; }

        public enum Primitives
        {
            STRING,
            BOOLEAN,
            INT,
            LONG,
            DECIMAL,
            DATETIME,
        }
    }
}
