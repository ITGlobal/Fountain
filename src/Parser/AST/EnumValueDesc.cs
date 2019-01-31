using System;
using System.Reflection;

namespace ITGlobal.Fountain.Parser
{
    public class EnumValueDesc
    {
        public string Description { get; set; }
        public bool IsDeprecated { get; set; }
        public object Value { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public Type EnumType { get; set; }
    }
}