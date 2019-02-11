using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
    public class ContractAttribute : Attribute, IBaseAttribute
    {
        public ContractAttribute(string group, string permission)
        {
            Group = group;
            Permission = permission;
        }

        public string Group { get; }
        public string Permission { get; }
        
        /// <summary>
        /// need for C# classes
        /// </summary>
        public bool CanBePartial { get; set; } = false;
    }
}