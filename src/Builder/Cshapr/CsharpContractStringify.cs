using ITGlobal.Fountain.Parser;

namespace ITGlobal.Fountain.Builder.Cshapr
{
    public class CsharpContractStringify: IContractStringify
    {
        private readonly IContractFieldStringify _fieldStringify;
        private readonly CsharpEmitterOptions _options;

        public CsharpContractStringify(IContractFieldStringify fieldStringify, CsharpEmitterOptions options)
        {
            _fieldStringify = fieldStringify;
            _options = options;
        }
        
        public string Stringify(ContractDesc contractDesc)
        {
            return $@"public class {_options.ContractNameTempate(contractDesc)} {{ }}";
        }
    }
}