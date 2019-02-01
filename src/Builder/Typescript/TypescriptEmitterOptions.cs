
using ITGlobal.Fountain.Builder.Exceptions;


namespace ITGlobal.Fountain.Builder.Typescript
{
    public class TypescriptEmitterOptions: EmitterOptions
    {
        public string Namespace { get; set; } = "Contracts";
        public string Module { get; set; } = "contracts";
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