namespace ConsoleApp.Assert;

public class AssertException : Exception
{
    public AssertException(string message) : base(message) { }
}