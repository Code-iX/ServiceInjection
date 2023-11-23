using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp;

[ServiceInjection]
public partial class TestClass_OneField_OneProperty_WithCtor_WithInjectedType
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterfaceInjected;

    [Injected]
    public ClassWithoutInterface ClassWithoutInterfaceInjected { get; set; }

    private readonly bool _ctorWasCalled;

    private TestClass_OneField_OneProperty_WithCtor_WithInjectedType()
    {
        _ctorWasCalled = true;
    }
}