using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp;

[ServiceInjection]
public partial class TestClass_OneProperty
{
    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }
}