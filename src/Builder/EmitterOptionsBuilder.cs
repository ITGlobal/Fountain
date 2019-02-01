using System;
using ITGlobal.Fountain.Builder.Typescript;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace ITGlobal.Fountain.Builder
{
    public abstract class EmitterOptionsBuilder<TOptions> : IEmitterOptionsBuilder<TOptions>, IEmitterOptionsBuilderSetup
        where TOptions : EmitterOptions, new()
    {
        [CanBeNull] private readonly Action<IEmitterOptionsBuilderSetup> _setup;
        protected readonly IServiceCollection _serviceCollection = new ServiceCollection();

        public EmitterOptionsBuilder([CanBeNull] Action<IEmitterOptionsBuilderSetup> setup = null)
        {
            _setup = setup;
        }

        public abstract void SetOneContractWrapper();
        public abstract void SetManyContractsWrapper();
        public abstract void SetFieldStringify();
        public abstract void SetContractStringify();
        public abstract void SetParserOptions();
        public abstract void SetParser();
        public abstract void SetContractEnumStringify();
        public abstract void SetEnumFieldStringify();
        public abstract TOptions Build();

        public TOptions BuildBase(TOptions options)
        {
            options.IdentSize = 4;
            options.FieldNamingStrategy = new DefaultNamingStrategy();
            options.ContractNameTempate = (contract) => contract.Name;
            _serviceCollection.AddSingleton(options);
            _serviceCollection.AddSingleton<IEmitterOptions>(options);
            _setup?.Invoke(this);

            var provider = _serviceCollection.BuildServiceProvider();
            options.PerFileContractWrapper = provider.GetService<IPerFileContractWrapper>();
            options.ManyContractsWrapper = provider.GetService<IManyContractsWrapper>();
            options.FieldStringify = provider.GetService<IContractFieldStringify>();
            options.ContractStringify = provider.GetService<IContractStringify>();
            options.ParserOptions = provider.GetService<IParserOptions>();
            options.Parser = provider.GetService<IParserAssembly>();
            options.ContractEnumStringify = provider.GetService<IContractEnumStringify>();
            options.EnumFieldStringify = provider.GetService<IEnumFieldStringify>();

            return options;
        }

        public void SetFieldStringify<T>() where T : class, IContractFieldStringify
        {
            _serviceCollection.AddSingleton<IContractFieldStringify, T>();
        }

        public void SetContractStringify<T>() where T : class, IContractStringify
        {
            _serviceCollection.AddSingleton<IContractStringify, T>();
        }

        public void SetOneContractWrapper<T>() where T : class, IPerFileContractWrapper
        {
            _serviceCollection.AddSingleton<IPerFileContractWrapper, T>();
        }

        public void SetManyContractsWrapper<T>() where T : class, IManyContractsWrapper
        {
            _serviceCollection.AddSingleton<IManyContractsWrapper, T>();
        }

        public void SetParserOptions<T>() where T : class, IParserOptions
        {
            _serviceCollection.AddSingleton<T>();
            _serviceCollection.AddSingleton<IParserOptions, T>();
        }

        public void SetParser<T>() where T : class, IParserAssembly
        {
            _serviceCollection.AddSingleton<IParserAssembly, T>();
        }

        public void SetContractEnumStringify<T>() where T : class, IContractEnumStringify
        {
            _serviceCollection.AddSingleton<IContractEnumStringify, T>();
        }

        public void SetEnumFieldStringify<T>() where T : class, IEnumFieldStringify
        {
            _serviceCollection.AddSingleton<IEnumFieldStringify, T>();
        }
    }
}