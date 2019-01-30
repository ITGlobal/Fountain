using System;
using Microsoft.Extensions.DependencyInjection;

namespace ITGlobal.Fountain.Builder
{
    public abstract class EmitterOptionsBuilder: IEmitterOptionsBuilder
    {
        private readonly Action<EmitterOptionsBuilder, IEmitterOptions> _setup;
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        protected readonly IEmitterOptions options = new EmitterOptions
        {
            IdentSize = 4,
        };

        public EmitterOptionsBuilder(Action<EmitterOptionsBuilder, IEmitterOptions> setup)
        {
            _setup = setup;
        }

        public abstract void SetFileTemplate();
        public abstract void SetIdentSize();
        public abstract void SetOneContractWrapper();
        public abstract void SetManyContractsWrapper();
        public abstract void SetFieldStringify();
        public abstract void SetContractStringify();

        public IEmitterOptions Build()
        {
            _serviceCollection.AddSingleton(options);
            _setup(this, options);
            
            var provider = _serviceCollection.BuildServiceProvider();
            options.PerFileContractWrapper = provider.GetService<IPerFileContractWrapper>();
            options.ManyContractsWrapper = provider.GetService<IManyContractsWrapper>();
            options.FieldStringify = provider.GetService<IContractFieldStringify>();
            options.ContractStringify = provider.GetService<IContractStringify>();
            
            return options;
        }

        public void SetCodeWrapper<T>() where T : class, ICodeWrapper
        {
            _serviceCollection.AddSingleton<ICodeWrapper, T>();
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

    }
}