namespace ITGlobal.Fountain.Parser
{
    public class ContractFieldDesc
    {
        public string Name { get; set; }
        public bool IsDeprecated { get; set; }
        public string DeprecationCause { get; set; }
        public string Description { get; set; }
        public ITypeDesc Type { get; set; }
    }
}