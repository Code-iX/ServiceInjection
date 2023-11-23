using ConsoleApp.Assert;
using ConsoleApp.StubClasses;
using ConsoleApp.TestClasses;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient<ClassWithoutInterface>();
        services.AddTransient<IClassWithInterface, ClassWithInterface>();
        services.AddTransient<IClassWithMultipleInheritedClasses, ClassWithMultipleInheritedClasses1>();
        services.AddTransient<ClassWithMultipleInheritedClasses1>();
        services.AddTransient<TestClass>();
    })
    .Build();

var testClass = host.Services
    .GetRequiredService<TestClass>();

// assert that all dependencies are injected
Assert.PrivateMemberNotNull(testClass, "_classWithoutInterface");
Assert.MemberNotNull(testClass, x => x.ClassB);
