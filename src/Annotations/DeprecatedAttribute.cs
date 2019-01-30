using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public class DeprecatedAttribute: Attribute, IBaseAttribute
    {
        public string Cause { get; }

        public DeprecatedAttribute(string cause)
        {
            Cause = cause.Trim();
        }
    }
}