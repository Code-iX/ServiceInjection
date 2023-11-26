[![Master](https://github.com/Code-iX/ServiceInjection/actions/workflows/master.yml/badge.svg)](https://github.com/Code-iX/ServiceInjection/actions/workflows/master.yml) [![Release](https://github.com/Code-iX/ServiceInjection/actions/workflows/release.yml/badge.svg)](https://github.com/Code-iX/ServiceInjection/actions/workflows/release.yml) [![NuGet](https://img.shields.io/nuget/v/CodeIX.ServiceInjection.svg)](https://www.nuget.org/packages/CodeIX.ServiceInjection/) [![GitHub](https://img.shields.io/github/license/Code-iX/ServiceInjection)](LICENSE.md) 

# CodeIX.ServiceInjection

The `CodeIX.ServiceInjection` is a Roslyn-based source generator for .NET that facilitates automatic dependency injection into classes. It allows services and dependencies to be inserted into classes without manually initializing them in the constructor.

## Table of Contents

- [Motivation](#motivation)- 
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [License](#license)
- [Contributing](#contributing)
- [Support](#support)

## Motivation

Dependency injection is a powerful technique for creating loosely coupled applications. However, it can be tedious to manually inject dependencies into classes. This is especially true for large projects with many dependencies. The `CodeIX.ServiceInjection` package aims to solve this problem by automatically injecting services and dependencies into classes.

Usually you would have to write something like this:

```csharp
public class MyClass
{
    private readonly MyService1 _myService1;
    private readonly MyService2 _myService2;
    private readonly MyService3 _myService3;
    
    public MyClass(MyService1 myService1, MyService2 myService2, MyService3 myService3)
    {
        _myService1 = myService1;
        _myService2 = myService2;
        _myService3 = myService3;
    }
}
```

A lot of boilerplate code is required to inject the services into the class. This is where the `CodeIX.ServiceInjection` package comes in. It automatically injects the services into the class by creating a partial class with the necessary constructor. 

## Features

- **Automatic Injection**: Injects services and dependencies directly into your classes.
- **Configurable**: Supports custom settings to control the injection process.
- **Easy to Use**: Seamlessly integrates into the .NET build process.

## Installation

Add the `CodeIX.ServiceInjection` NuGet package to your project:

```csharp
dotnet add package CodeIX.ServiceInjection
```

## Usage

To use the source generator, simply mark a class with the `[ServiceInjection]` attribute and mark
the properties or fields you want to be injected with the `[Injected]` attribute:

```csharp
using CodeIX.ServiceInjection;

[ServiceInjection]
public partial class MyClass
{
    [Injected(false)]
    public MyService MyServiceObject { get; set; }
}
```

The source generator automatically generates a partial class with the necessary constructor to inject `MyService` into `MyClass`, which 
will look like this:

```csharp
partial class MyClass
{
    public MyClass(MyService MyServiceObject)
    {
        this.MyServiceObject = MyServiceObject ?? throw new ArgumentNullException(nameof(MyServiceObject));
    }
}
```


## Examples

### Basic Usage

ServiceInjection is possible for both properties and fields:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    [Injected]
    private readonly Service1 _service1Field;

    [Injected]
    public Service2 Service2Property { get; set; }
}
```

### Optional Dependencies

Dependencies can be marked as optional by setting the `isOptional` parameter of the `[Injected]` attribute to `true`:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    [Injected]
    public Service1 Service1Property { get; set; }

    [Injected(isOptional: true)]
    public Service2 Service2Property { get; set; }
}
```

If Service1 is not registered, the ServiceProvider will throw an exception. If the optional Service2 is not registered in the service collection, the ServiceProvider will inject `null` into the property. 

### Setting the injected type

By default, the type of the injected property or field is used to determine the type of the injected service. If you want to inject a different type, you can set the `injectedType` parameter of the `[Injected]` attribute:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    [Injected(injectedType: typeof(Service1))]
    public IService Service1Property { get; set; } // will inject Service1
    
    [Injected(injectedType: typeof(Service2))]
    public IService Service2Property { get; set; } // will inject Service2
}
```

_Keep in mind that the injected type must be registered in the service collection by its inherited type instead of base type._

### Custom Constructor

You can still create a constructor for your class. The source generator will call it and inject the services afterwards:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    public ExampleClass()
    {
        // do something
        // keep in mind that the injected services are not available here
    }

    [Injected]
    public Service1 Service1Property { get; set; }
}
```

You can also create a constructor with parameters. The source generator will call it and inject the services afterwards:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    public ExampleClass(Service1 service1Property)
    {
        // do something with service1Property before it is injected, like configuration
    }

    [Injected]
    Service1 Service1Property { get; set; }
}
```

_Keep in mind that the injected services are injected after the constructor is called. This means that the injected services are not available in the constructor._

Also it's constrained by the ServiceProviders ability to resolve the constructor parameters. If the ServiceProvider can't resolve the constructor parameters, it will throw an exception.

### List of Services

Of course you can also inject a list of services, which will give you all Services of the specified type:

```csharp
[ServiceInjection]
public partial class ExampleClass
{
    [Injected]
    public IEnumerable<IService> ServiceProperty { get; set; }
}
```

_This must be supported by the ServiceProvider. For example, the ServiceProvider of Microsoft.Extensions.DependencyInjection supports this._

## License

This project is licensed under the [MIT License](LICENSE.md).

## Contributing

Contributions are welcome! Please read our [contribution guidelines](CONTRIBUTING.md) for more information.

### Roadmap

- [x] Property injection
- [x] Field injection
- [ ] Constructor parameter injection
- [ ] Support registering services in the service collection

## Support

For questions or issues, please open an [issue](https://github.com/Code-iX/ServiceInjection/issues) on GitHub.
