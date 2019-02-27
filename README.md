# Fountain
Code generation library for contracts

[![Build status](https://ci.appveyor.com/api/projects/status/7lplwn0fsismdm0f?svg=true)](https://ci.appveyor.com/project/itgloballlc/fountain)

Nuget
* ITGlobal.Fountain.Builder <br> [![ITGlobal.Fountain.Builder](https://img.shields.io/nuget/v/ITGlobal.Fountain.Builder.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Builder/)
* ITGlobal.Fountain.Parser <br> [![ITGlobal.Fountain.Parser](https://img.shields.io/nuget/v/ITGlobal.Fountain.Parser.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Parser/)
* ITGlobal.Fountain.Annotations <br> [![ITGlobal.Fountain.Annotations](https://img.shields.io/nuget/v/ITGlobal.Fountain.Annotations.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Annotations/)

## Usage
Create contracts.
```C#
[Contract("product", "PUBLIC")]
[Documentation("some description for contract")]
public class Product: MetadataObject
{
    [Documentation("Product Code")]
    [Required("ON_SERVER")]
    [ContractField("PUBLIC")]
    public string Code { get; set; }

    [Documentation("Product Name")]
    [ContractField("PUBLIC")]
    [Required("ON_SERVER")]
    public LocalizedText Name { get; set; }

    [Documentation("Product prices table")]
    [ContractField("MANAGER")]
    public Dictionary<PriceRegion, PriceDefenition> Prices { get; set; }
}

// abstract classes don't emit
[Contract("common", "PUBLIC")]
[Documentation("Метаданные")]
public abstract class MetadataObject

{
    [Documentation("Id")]
    [ContractField("PUBLIC"), Nullable]
    public string Id { get; set; }
    
    [Documentation("Some metadata")]
    [JsonProperty("$metadata")]
    [ContractField("PUBLIC")]
    [Nullable]
    public Dictionary<string, object> Metadata { get; set; }
}

[Contract("product", "MANAGER")]
[Documentation("Price regions")]
public enum PriceRegion
{
    [Documentation("Россия")]
    [JsonName("RUSSIA")]
    [ContractField("MANAGER")]
    RUSSIA,
    
    [Documentation("США")]
    [JsonName("USA")]
    [ContractField("MANAGER")]
    USA,
}

[Contract("product", "MANAGER")]
[Documentation("Some description of price definition")]
public class PriceDefenition
{
    [Documentation("Price")]
    [ContractField("MANAGER")]
    public float Price { get; set; }

    [Documentation("Price Currency")]
    [ContractField("MANAGER")]
    public string Currency { get; set; }
}
``` 
`[Contract("product", "PUBLIC")]` - this means that `Product` contract belongs to `product` group and has `PUBLIC` permission.
`[ContractField("PUBLIC")]` - this means that field is an contract field and belongs to `Product` contract.
`[Required("ON_SERVER")]` - this means that field required on server when server execute validation.
`[JsonName("USA")]` - this defines how the field is called during serialization.

Emit contracts for typescript and C#
```C#
// get assemly with contracts
var assebly = typeof(Product).Assembly;
// emit typescript contracts with default settings
FileEmitterBuilder.Build(new TypescriptEmitterOptionsBuilder())
    .SetupOptions(options => {
        // Filename or FileTemplate option required
        options.Filename = "contracts.d.ts";
        // filter only public contracts
        options.ParserOptions.FilterContracts = attr => attr.Permission == "PUBLIC";
        // filter only public fields
        options.ParserOptions.FilterFields = attr => attr.Permission == "PUBLIC";
    })
    .Emit("path/to/typescript/output/folder", assebly);
// emit C# contracts with default settings
FileEmitterBuilder.Build(new CsharpEmitterOptionsBuilder())
    .SetupOptions(options => {
        // Filename or FileTemplate option required
        options.Filename = "Contracts.cs";
        // filter only public contracts
        options.ParserOptions.FilterContracts = attr => attr.Permission == "PUBLIC";
        // filter only public fields
        options.ParserOptions.FilterFields = attr => attr.Permission == "PUBLIC";
    })
    .Emit("path/to/C#/output/folder", assebly);
```

Result in `contracts.d.ts`
```typescript
// todo
```

Result in `Contracts.cs`
```C#
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ITGlobal.Promise.RestAPI.Contracts.Mgr {
    /// <summary>
    /// some description for contract
    /// </summary>
    public class ProductContract {

        /// <summary>
        /// Product Code
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [Required]
        public LocalizedText Name { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [CanBeNull]
        public string Id { get; set; }

        /// <summary>
        /// Some metadata
        /// </summary>
        [CanBeNull]
        public Dictionary<string, Object> Metadata { get; set; }

    }
}
```

## Options
Options setup has 2 steps.

### First. Builder. 
There is a possibility redefine default classes for parser and emitter.
```C#
FileEmitterBuilder.Build(new TypescriptEmitterOptionsBuilder(builder => {
    builder.SetParser<MyCustomParser>();
    builder.SetFieldStringify<MyCustomTypescriptFieldStringify>;
}))
```

See all available builder options [here](https://github.com/ITGlobal/Fountain/blob/master/src/Builder/IEmitterOptionsBuilderSetup.cs)

### Second. Static options
Use SetupOptions to set template for files, filtration, ident, and etc.
```C#
FileEmitterBuilder.Build(new TypescriptEmitterOptionsBuilder(builder => {
    builder.SetParser<MyCustomParser>();
    builder.SetFieldStringify<MyCustomTypescriptFieldStringify>;
})).SetupOptions((options) => {
    options.FieldNamingStrategy = new CamelCaseNamingStrategy();
    options.ContractNameTempate = desc => $"Mgr{desc.Name}";
    options.IdentSize = 4;
    options.Filename = "mgr-contracts.d.ts";
    options.TypescriptModuleType = TypescriptModuleType.Namespace;
    options.Namespace = "MgrContracts";
    options.ParserOptions.FilterContracts = attr =>
        attr.Permission == ContractPermission.PUBLIC || attr.Permission == ContractPermission.MANAGER;
    options.ParserOptions.FilterFields = attr =>
        attr.Permission == ContractPermission.PUBLIC || attr.Permission == ContractPermission.MANAGER;
})
```

### Options reference
* [common options](https://github.com/ITGlobal/Fountain/blob/master/src/Builder/IEmitterOptions.cs)
* [typescript options](https://github.com/ITGlobal/Fountain/blob/master/src/Builder/Typescript/TypescriptEmitterOptions.cs)
* [C# options](https://github.com/ITGlobal/Fountain/blob/master/src/Builder/Csharp/CsharpEmitterOptions.cs)


## Roadmap
* [x] Enums
* [x] Generics
* [x] Optional
* [x] C# builder
* [x] Filter by permission
* [x] Filter by group
* [x] camelCase dash_case settings
* [x] Modular parser
* [ ] Get summary as description
* [x] Deprecation
* [x] Parser cache
* [ ] Support perfile modules for typescript
* [x] Parser - group by base class
* [ ] Emitter hooks
* [ ] Tests
