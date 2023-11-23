using System;
using System.ComponentModel;

namespace CodeIX.ServiceInjection;

/// <summary>
/// Marks a property or field to be injected by the service provider via constructor.
/// Note: This attribute should only be applied to properties or fields within classes,
/// interfaces, or structs that are also marked with the ServiceInjection attribute
/// and are declared as partial.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
[Description("Mark properties and fields to be injected. Optionally specify a type to be injected. Should only be used within partial classes/interfaces/structs with ServiceInjection attribute.")]
public class InjectedAttribute : Attribute
{
    public bool IsOptional { get; set; }
    public Type InjectedType { get; set; }

    public InjectedAttribute() { }

    public InjectedAttribute(bool isOptional)
    {
        IsOptional = isOptional;
    }

    public InjectedAttribute(Type injectedType)
    {
        InjectedType = injectedType;
    }

    public InjectedAttribute(bool isOptional, Type injectedType)
    {
        IsOptional = isOptional;
        InjectedType = injectedType;
    }
}