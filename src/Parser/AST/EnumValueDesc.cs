using System;
using System.Reflection;

namespace ITGlobal.Fountain.Parser
{
    public class EnumValueDesc: IDeprecatableDesc, IDescriptableDesc
    {
        public string Description { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public object Value { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public Type EnumType { get; set; }
        public string JsonName { get; set; }
    }
}