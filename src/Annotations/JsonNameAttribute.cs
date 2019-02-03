using System;

namespace ITGlobal.Fountain.Annotations
{
    /// <summary>
    /// Optional. Mark enum values and contract propertries to define json field names statically 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonNameAttribute: Attribute, IBaseAttribute
    {
        public string Name { get; }

        public JsonNameAttribute(string name)
        {
            Name = name;
        }
    }
}