using CodeIX.ServiceInjection;

namespace MyNamespace
{
    [ServiceInjection]
    partial class MyName
    {
        [Injected]
        private readonly object MyDependentName;

    }
}