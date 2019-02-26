
using ITGlobal.Fountain.Builder.Exceptions;


namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEmitterOptions: EmitterOptions
    {
        /// <summary>
        /// Set namespace for TypescriptModuleType.Namespace mode <see cref="TypescriptModuleType"/>
        /// </summary>
        public string Namespace { get; set; } = "Contracts";
        
        /// <summary>
        /// Set module name for TypescriptModuleType.Module mode <see cref="TypescriptModuleType"/>
        /// </summary>
        public string Module { get; set; } = "contracts";
        
        /// <summary>
        /// Emitter mode
        /// </summary>
        public TypescriptModuleType TypescriptModuleType { get; set; } = TypescriptModuleType.Namespace;

        public override void CheckOptions()
        {
            base.CheckOptions();
            if (TypescriptModuleType == TypescriptModuleType.Module && FileTemplate != null)
            {
                throw new CheckOptionsException(new[] {nameof(TypescriptModuleType), nameof(FileTemplate)},
                    "Perfile contracts to module not suppotred");
            }
        }
    }

    public enum TypescriptModuleType
    {
        Namespace,
        Module
    }
}