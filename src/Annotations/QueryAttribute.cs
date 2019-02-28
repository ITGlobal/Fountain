using System;

namespace ITGlobal.Fountain.Annotations
{
    /// <summary>
    /// Optional. Mark contract properties to define query string parameter names statically 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class QueryAttribute : Attribute, IBaseAttribute
    {
        public string Name { get; }

        public QueryAttribute(string name)
        {
            Name = name;
        }
    }
}