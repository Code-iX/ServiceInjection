using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp;

[ServiceInjection]

public partial class TestClass_TwoProperties
{
    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }

    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected2 { get; set; }
}