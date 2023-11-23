using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass_TwoFields
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;

    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected2;
}