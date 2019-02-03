using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class TypeNameAttribute : Attribute, IBaseAttribute
    {
        public TypeNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}