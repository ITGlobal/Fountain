using System;

namespace ITGlobal.Fountain.Annotations
{
    /// <summary>
    /// If need emit custom attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class EmitAttributeAttribute: Attribute, IBaseAttribute
    {
        public string AttributeStr { get; }
        public string Destination { get; }

        /// <param name="attributeStr">string with attribute defenition</param>
        /// <param name="destination">can be "ON_SERVER", "ON_CLIENT", "MANAGER" e.g.</param>
        public EmitAttributeAttribute(string attributeStr, string destination)
        {
            AttributeStr = attributeStr;
            Destination = destination;
        }
    }
}