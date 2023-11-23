using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]

public partial class TestClass_TwoProperties
{
    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }

    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected2 { get; set; }
}