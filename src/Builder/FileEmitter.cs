using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ITGlobal.Fountain.Parser;
using JetBrains.Annotations;

namespace ITGlobal.Fountain.Builder
{
    public class FileEmitter<TOptions>: IFileEmitter<TOptions> where TOptions: IEmitterOptions
    {
        private readonly TOptions _options;

        public FileEmitter(TOptions options)
        {
            _options = options;
        }

        public FileEmitter<TOptions> SetupOptions(Action<TOptions> setup)
        {
            setup(_options);
            return this;
        }

        public void Emit(string output, Assembly assembly)
        {
            _options.CheckOptions();
            var group = _options.Parser.Parse(assembly);
            if (_options.FileTemplate != null)
            {
                EmitFilePerContract(output, group);
            }
            else
            {
                EmitOneFile(output, group);
            }
        }

        private void EmitOneFile([NotNull]string output, [NotNull]ContractGroup parsed)
        {
            var buffer = new List<string>();
            foreach (var group in parsed.Groups)
            {
                foreach (var contract in group.Value)
                {
                    var contractStr = "";
                    // render contract string
                    if (contract is ContractDesc cd)
                        contractStr = _options.ContractStringify.Stringify(cd);
                    if (contract is ContractEnumDesc ed)
                        contractStr = _options.ContractEnumStringify.Stringify(ed);
                    if (!string.IsNullOrWhiteSpace(contractStr))
                    {
                        buffer.Add(_options.ManyContractsWrapper.WrapOne(contractStr));
                    }
                }
            }

            var result = string.Join(Environment.NewLine+Environment.NewLine, buffer);
            var wrappedStr = _options.ManyContractsWrapper.WrapAll(result, parsed);
            var filename = _options.Filename;
            if (filename == null)
            {
                throw new Exception("filename must be defined");
            }
            var filepath = Path.Combine(output, filename);
            WriteToFile(filepath, wrappedStr);
        }
        
        private void EmitFilePerContract([NotNull]string output, [NotNull]ContractGroup parsed)
        {
            if (_options.FileTemplate == null)
            {
                throw new Exception("FileTemplate option must be defined");
            }
            
            foreach (var group in parsed.Groups)
            {
                foreach (var contract in group.Value)
                {
                    var contractStr = "";
                    // render contract string
                    if (contract is ContractDesc cd)
                        contractStr = _options.ContractStringify.Stringify(cd);
                    if (contract is ContractEnumDesc ed)
                        contractStr = _options.ContractEnumStringify.Stringify(ed);

                    if (!string.IsNullOrWhiteSpace(contractStr))
                    {
                        var wrappedStr = _options.PerFileContractWrapper.Wrap(contractStr);
                        var filename = _options.FileTemplate(group.Key, contract);
                        if (filename == null)
                        {
                            throw new Exception("filename must be defined");
                        }
                        var filepath = Path.Combine(output, filename);
                        WriteToFile(filepath, wrappedStr);
                    }
                }
            }
        }

        private FileStream CreateFileIfNotExist([NotNull]string filepath)
        {
            if (!File.Exists(filepath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? throw new Exception("directory can't be null"));
            }

            return File.OpenWrite(filepath);
        }

        private void WriteToFile([NotNull] string filepath, [NotNull]string data)
        {
            using (var file = CreateFileIfNotExist(filepath))
            {
                // clean file before write
                file.SetLength(0);
                var bytes = new UTF8Encoding(true).GetBytes(data);  
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}