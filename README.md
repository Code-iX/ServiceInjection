# ServiceInjection

The `ServiceInjection` is a Roslyn-based source generator for .NET that facilitates automatic dependency injection into classes. It allows services and dependencies to be inserted into classes without manually initializing them in the constructor.

## Features

- **Automatic Injection**: Injects services and dependencies directly into your classes.
- **Configurable**: Supports custom settings to control the injection process.
- **Easy to Use**: Seamlessly integrates into the .NET build process.

## Installation

Add the `ServiceInjection` NuGet package to your project:

```csharp
dotnet add package ServiceInjection
```

## Usage

To use the source generator, simply mark the properties or fields you want to be injected with the `[Injected]` attribute:

```csharp
[ServiceInjection]
public partial class MyClass
{
    [Injected]
    private readonly MyService _myService;
}
```

The source generator automatically generates the necessary constructor to inject `MyService` into `MyClass`.

## Examples

A simple usage example:

```csharp
[ServiceInjection]
public partial class SampleClass
{
    [Injected]
    private readonly MyDependency _myDependency;
}
```

For more examples and advanced use cases, please refer to the documentation.

## License

This project is licensed under the [MIT License](LICENSE.md).

## Contributing

Contributions are welcome! Please read our [contribution guidelines](CONTRIBUTING.md) for more information.

## Support

For questions or issues, please open an [issue](https://github.com/Matt-17/ServiceInjection/issues) on GitHub.
