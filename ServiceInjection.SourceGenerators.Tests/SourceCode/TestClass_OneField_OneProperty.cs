using CodeIX.ServiceInjection;

[ServiceInjection]
public partial class TestClass_OneField_OneProperty
{
    [Injected]
    private Class _field;

    [Injected]
    public Class Property { get; set; }
}

public class Class { }