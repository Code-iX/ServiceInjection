using CodeIX.ServiceInjection;

[ServiceInjection]
public partial class TestClass_OneField
{
    [Injected]
    private Class _field;
}

public class Class { }