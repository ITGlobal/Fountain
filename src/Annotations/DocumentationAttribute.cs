using System;

namespace ITGlobal.Fountain.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class DocumentationAttribute : Attribute, IBaseAttribute
    {
        public DocumentationAttribute(string text)
        {
            Text = text.Trim();
        }

        public string Text { get; }
        public string Example { get; set; }
    }
}