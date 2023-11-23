using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass_OneField_OneProperty
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;

    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }
}