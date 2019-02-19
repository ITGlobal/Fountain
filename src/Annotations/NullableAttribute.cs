using System;

namespace ITGlobal.Fountain.Annotations
{
    /// <summary>
    /// Declare that field can be null
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NullableAttribute: Attribute, IBaseAttribute
    {
        
    }
}