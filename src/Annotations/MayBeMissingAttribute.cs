using System;

namespace ITGlobal.Fountain.Annotations
{
    /// <summary>
    /// Declare that field may be missing in recieved contract 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MayBeMissingAttribute: Attribute, IBaseAttribute
    {
        
    }
}