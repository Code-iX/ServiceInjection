using ConsoleApp.StubClasses;
using ServiceInjection;

namespace ConsoleApp.TestClasses;

[ServiceInjection]
public partial class TestClass
{
    [Injected]
    private ClassWithoutInterface _classWithoutInterface;

    private readonly bool _ctorWasCalled;

    private TestClass(IClassWithMultipleInheritedClasses classWithMultipleInheritedClasses, IClassWithInterface clb, ClassWithoutInterface abc)
    {
        _ctorWasCalled = true;
    }

    [Injected(false)]
    public IClassWithInterface ClassB { get; set; }

    [Injected(typeof(ClassWithMultipleInheritedClasses1))]
    public IClassWithMultipleInheritedClasses ClassWithMultipleInheritedClasses { get; set; }
}
