using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleApp.Assert;

public static class Assert
{
    public static void PrivateMemberNotNull<T>(T obj, string fieldName)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException(nameof(fieldName));

        var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null) throw new ArgumentException($"Field '{fieldName}' not found in type {obj.GetType()}.");

        var value = fieldInfo.GetValue(obj);
        if (value == null) throw new AssertException($"Private field '{fieldName}' is null.");
    }

    public static void MemberNotNull<T>(T testClass, Expression<Func<T, object>> func)
    {
        if (testClass == null) throw new ArgumentNullException(nameof(testClass));
        if (func == null) throw new ArgumentNullException(nameof(func));

        var memberExpression = func.Body as MemberExpression;
        if (memberExpression == null) throw new ArgumentException($"Expression '{func}' is not a member expression.");

        var memberInfo = memberExpression.Member;
        if (memberInfo == null) throw new ArgumentException($"Expression '{func}' does not contain a member.");

        var value = memberInfo switch
        {
            PropertyInfo propertyInfo => propertyInfo.GetValue(testClass),
            FieldInfo fieldInfo => fieldInfo.GetValue(testClass),
            _ => throw new ArgumentException($"Expression '{func}' is not a property or field.")
        };

        if (value == null) throw new AssertException($"Member '{memberInfo.Name}' is null.");


    }
}