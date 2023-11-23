using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp;

[ServiceInjection]
public partial class TestClass_TwoFields
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;

    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected2;
}