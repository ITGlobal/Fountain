namespace ITGlobal.Fountain.Parser
{
    public interface IDeprecatableDesc
    {
        bool IsDeprecated { get; set; }
        string DeprecationCause { get; set; }
    }
}