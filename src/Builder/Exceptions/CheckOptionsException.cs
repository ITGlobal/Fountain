using System;
using System.Collections.Generic;

namespace ITGlobal.Fountain.Builder.Exceptions
{
    public class CheckOptionsException: Exception
    {
        public IEnumerable<string> Fields { get; }

        public CheckOptionsException(IEnumerable<string> fields, string message): base(message)
        {
            Fields = fields;
        }   
    }
}