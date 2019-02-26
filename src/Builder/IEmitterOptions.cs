using System;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    /// <summary>
    /// Common emitter options
    /// </summary>
    public interface IEmitterOptions
    {
        /// <summary>
        /// Indentation size in spaces. 
        /// </summary>
        /// <example>
        /// <code>options.IdentSize = 4</code>
        /// </example>
        int IdentSize { get; set; }
        
        /// <summary>
        /// Set Filename option to emit one file with contracts
        /// </summary>
        /// <example>
        /// <code>options.Filename = "contracts.d.ts"</code>
        /// </example>
        [CanBeNull] string Filename { get; set; }
        
        /// <summary>
        /// Set FileTemplate option to emit contract per file
        /// </summary>
        /// <example>
        /// <code>options.FileTemplate = (group, typedesc) => $"Mgr/{group}/Mgr{typedesc.Name}.cs";</code>
        /// </example>
        [CanBeNull] Func<string, ITypeDesc, string> FileTemplate { get; set; }
        
        /// <summary>
        /// Set FieldNamingStrategy option to convert field names in emitted contracts 
        /// </summary>
        /// <example>
        /// <code>options.FieldNamingStrategy = new CamelCaseNamingStrategy();</code>
        /// </example>
        [NotNull] NamingStrategy FieldNamingStrategy { get; set; }
        
        /// <summary>
        /// Set ContractNameTempate option to transform contract names
        /// </summary>
        /// <example>
        /// <code>options.ContractNameTempate = desc => $"Mgr{desc.Name}";</code>
        /// </example>
        [NotNull] Func<ITypeDesc, string> ContractNameTempate { get; set; }
        
        /// <summary>
        /// Set per file code wrapper
        /// </summary>
        /// <example>
        /// <code>builder.SetOneContractWrapper&lt;MyCustomWrapper&gt;()</code>
        /// or 
        /// <code>options.PerFileContractWrapper = new MyCustomWrapper(...);"</code>
        /// </example>
        [NotNull] IPerFileContractWrapper PerFileContractWrapper { get; set; }
        
        /// <summary>
        /// Set code wrapper for one file mode
        /// </summary>
        /// <example>
        /// <code>builder.SetManyContractsWrapper&lt;MyCustomWrapper&gt;()</code>
        /// or 
        /// <code>options.ManyContractsWrapper = new MyCustomWrapper(...);"</code>
        /// </example>
        [NotNull] IManyContractsWrapper ManyContractsWrapper { get; set; }
        
        /// <summary>
        /// Set contract field stringifier class
        /// </summary>
        /// <example>
        /// <code>builder.SetFieldStringify&lt;MyCustomFieldStringify&gt;()</code>
        /// or 
        /// <code>options.FieldStringify = new MyCustomFieldStringify(...);"</code>
        /// </example>
        [NotNull] IContractFieldStringify FieldStringify { get; set; }
        
        /// <summary>
        /// Set contract stringifier class
        /// </summary>
        /// <example>
        /// <code>builder.SetContractStringify&lt;MyCustomContractStringify&gt;()</code>
        /// or 
        /// <code>options.ContractStringify = new MyCustomContractStringify(...);"</code>
        /// </example>
        [NotNull] IContractStringify ContractStringify { get; set; }
        
        /// <summary>
        /// Set enum stringifier class
        /// </summary>
        /// <example>
        /// <code>builder.SetContractEnumStringify&lt;MyCustomContractEnumStringify&gt;()</code>
        /// or 
        /// <code>options.ContractEnumStringify = new MyCustomContractEnumStringify(...);"</code>
        /// </example>
        [NotNull] IContractEnumStringify ContractEnumStringify { get; set; }
        
        /// <summary>
        /// Set enum item stringifier class
        /// </summary>
        /// <example>
        /// <code>builder.SetEnumFieldStringify&lt;MyCustomEnumFieldStringify&gt;()</code>
        /// or 
        /// <code>options.EnumFieldStringify = new MyCustomEnumFieldStringify(...);"</code>
        /// </example>
        [NotNull] IEnumFieldStringify EnumFieldStringify { get; set; }
        
        /// <summary>
        /// Set stringifier class for generic contracts
        /// </summary>
        /// <example>
        /// <code>builder.SetContractGenericStringify&lt;MyCustomContractGenericStringify&gt;()</code>
        /// or 
        /// <code>options.ContractGenericStringify = new MyCustomContractGenericStringify(...);"</code>
        /// </example>
        [NotNull] IContractGenericStringify ContractGenericStringify { get; set; }
        
        /// <summary>
        /// Set stringifier class for generic contracts
        /// </summary>
        /// <example>
        /// <code>builder.SetParserOptions&lt;MyCustomParserOptions&gt;()</code>
        /// or 
        /// <code>options.ParserOptions = new MyCustomParserOptions(...);"</code>
        /// </example>
        [NotNull] IParserOptions ParserOptions { get; set; }
        
        /// <summary>
        /// Set parser
        /// </summary>
        /// <example>
        /// <code>builder.SetParser&lt;MyCustomParser&gt;()</code>
        /// or 
        /// <code>options.Parser = new MyCustomParser(...);"</code>
        /// </example>
        [NotNull] IParserAssembly Parser { get; set; }

        /// <summary>
        /// options validator
        /// </summary>
        void CheckOptions();
    }
}