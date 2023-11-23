using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp;

[ServiceInjection]
public partial class TestClass_OneField
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;
}