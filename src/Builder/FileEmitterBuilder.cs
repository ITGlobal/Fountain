namespace ITGlobal.Fountain.Builder
{
    public class FileEmitterBuilder
    {
        public static FileEmitter<TOptions> Build<TOptions>(IEmitterOptionsBuilder<TOptions> builder) where TOptions: IEmitterOptions
        {
            builder.SetOneContractWrapper();
            builder.SetManyContractsWrapper();
            builder.SetFieldStringify();
            builder.SetContractStringify();
            builder.SetParserOptions();
            builder.SetParser();
            builder.SetEnumFieldStringify();
            builder.SetContractEnumStringify();
            return new FileEmitter<TOptions>(builder.Build());
        }
    }
}