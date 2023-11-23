using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass_OneField
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;
}