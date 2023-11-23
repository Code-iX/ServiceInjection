using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass_OneProperty
{
    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }
}