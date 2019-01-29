using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DocumentationAttribute : Attribute
    {
        public DocumentationAttribute(string text)
        {
            Text = text.Trim();
        }

        public string Text { get; }
    }
}