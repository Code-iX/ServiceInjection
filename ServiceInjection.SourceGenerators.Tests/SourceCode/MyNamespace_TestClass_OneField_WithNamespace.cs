using CodeIX.ServiceInjection;

namespace MyNamespace;

[ServiceInjection]
public partial class TestClass_OneField_WithNamespace
{
    [Injected]
    private Class _classInjected;
}

public class Class { }