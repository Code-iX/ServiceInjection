using CodeIX.ServiceInjection;
using ConsoleApp.StubClasses;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass_OneField_OneProperty_WithCtor
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;

    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }

    private readonly bool _ctorWasCalled;

    private TestClass_OneField_OneProperty_WithCtor()
    {
        _ctorWasCalled = true;
    }
}