using System;

namespace ITGlobal.Fountain.Annotations.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EmailAttribute: Attribute, IBaseAttribute
    {
        public string Destination { get; }

        /// <param name="destination">can be "ON_SERVER", "ON_CLIENT", "MANAGER" e.g.</param>
        public EmailAttribute(string destination)
        {
            Destination = destination;
        }
    }
}