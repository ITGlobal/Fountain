# Fountain
Code generation library for contracts

[![Build status](https://ci.appveyor.com/api/projects/status/7lplwn0fsismdm0f?svg=true)](https://ci.appveyor.com/project/itgloballlc/fountain)

Nuget
* ITGlobal.Fountain.Builder <br> [![ITGlobal.Fountain.Builder](https://img.shields.io/nuget/v/ITGlobal.Fountain.Builder.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Builder/)
* ITGlobal.Fountain.Parser <br> [![ITGlobal.Fountain.Parser](https://img.shields.io/nuget/v/ITGlobal.Fountain.Parser.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Parser/)
* ITGlobal.Fountain.Annotations <br> [![ITGlobal.Fountain.Annotations](https://img.shields.io/nuget/v/ITGlobal.Fountain.Annotations.svg)](https://www.nuget.org/packages/ITGlobal.Fountain.Annotations/)

## Install

## Usage
Create contract.
```C#
[Contract('product', 'PUBLIC')]
[Documentation("some description for contract")]
public class Product: MetadataObject
{
    [Documentation("Код продукта")]
    [Required('ON_SERVER')]
    [ContractField('PUBLIC')]
    public string Code { get; set; }

    [Documentation("Название продукта")]
    [ContractField('PUBLIC')]
    [Required('ON_SERVER')]
    public LocalizedText Name { get; set; }

    [Documentation("Таблица цен по регионам для продукта за месяц")]
    [ContractField('MANAGER')]
    public Dictionary<PriceRegion, PriceDefenition> Prices { get; set; }
}
``` 
`[Contract('product', 'PUBLIC')]` - this means that `Product` contract belongs to `product` group and has `PUBLIC` permission.
`[ContractField('PUBLIC')]` - this means that field is an contract field and belongs to `Product` contract.
`[Required('ON_SERVER')]` - this means that field required on server when server execute validation.

Emit contracts for typescript
```C#
// get assemly with contracts
var assebly = typeof(Product).Assembly;
// emit typescript contracts with default settings
FileEmitterBuilder.Build(new TypescriptEmitterOptionsBuilder())
    .SetupOptions(options => {
        // Filename or FileTemplate option required
        options.Filename = "contracts.d.ts";
    })
    .Emit("path/to/typescript/output/folder", assebly);
// emit C# contracts with default settings
FileEmitterBuilder.Build(new CsharpEmitterOptionsBuilder())
    .SetupOptions(options => {
        // Filename or FileTemplate option required
        options.Filename = "Contracts.cs";
    })
    .Emit("path/to/C#/output/folder", assebly);
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
