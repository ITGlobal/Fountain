using System;
using Scriban;
using Scriban.Runtime;

namespace ITGlobal.Fountain.Builder.Csharp
{
    /// <summary>
    /// Common Scriban template context
    /// </summary>
    public class CsharpTemplateContext
    {
        private readonly CsharpEmitterOptions _options;

        public CsharpTemplateContext(CsharpEmitterOptions options)
        {
            _options = options;
        }
        
        public TemplateContext Make(object obj)
        {
            var scriptObj = new ScriptObject();
            scriptObj.Import(obj);
            scriptObj.Import("ident", new Func<string, string>((str) => Utils.Ident(str, _options.IdentSize)));
            scriptObj.Add("left_pad", Utils.Ident("", _options.IdentSize));
            
            var context = new TemplateContext();
            context.PushGlobal(scriptObj);

            return context;
        }
    }
}