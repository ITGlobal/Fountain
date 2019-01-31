using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ContractFieldAttribute: Attribute, IBaseAttribute
    {
        public string Permission { get; }

        public ContractFieldAttribute(string permission)
        {
            Permission = permission;
        }
    }
}