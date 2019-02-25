using System.Collections.Generic;

namespace ITGlobal.Fountain.Parser
{
    /// <summary>
    /// Descriptions for constracted generic types.
    /// Constracted generic types used in base types and properties
    /// </summary>
    public class ConstructedGenericDesc : ITypeDesc
    {
        public string Name { get; set; }
        /// <summary>
        /// Generic arguments
        /// </summary>
        public IEnumerable<ITypeDesc> Arguments { get; set; }
    }
}