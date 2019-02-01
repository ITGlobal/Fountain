using System;

namespace ITGlobal.Fountain.Parser
{
    public sealed class PrimitiveDesc: ITypeDesc
    {
        public PrimitiveDesc(Primitives type)
        {
            Type = type;
            Name = GetName(type);
        }
        
        public string Name { get; }
        public Primitives Type { get; }
        
        public static string GetName(Primitives type) {
            switch (type)
            {
                case Primitives.STRING:
                    return "string";
                case Primitives.BOOLEAN:
                    return "bool";
                case Primitives.INT:
                    return "int";
                case Primitives.LONG:
                    return "long";
                case Primitives.DECIMAL:
                    return "decimal";
                case Primitives.FLOAT:
                    return "float";
                case Primitives.USHORT:
                    return "ushort";
                case Primitives.DATETIME:
                    return "datetime";
                case Primitives.BYTE:
                    return "byte";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum Primitives
        {
            STRING,
            BOOLEAN,
            INT,
            LONG,
            DECIMAL,
            FLOAT,
            USHORT,
            DATETIME,
            BYTE,
        }
    }
}
