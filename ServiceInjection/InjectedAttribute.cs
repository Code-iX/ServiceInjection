using System;
using System.ComponentModel;

namespace ServiceInjection
{
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
        public bool Required { get; set; } = true;
        public Type InjectedType { get; set; } = null;

        public InjectedAttribute() { }

        public InjectedAttribute(bool required)
        {
            Required = required;
        }

        public InjectedAttribute(Type injectedType)
        {
            InjectedType = injectedType;
        }

        public InjectedAttribute(bool required, Type injectedType)
        {
            Required = required;
            InjectedType = injectedType;
        }
    }

}
