﻿using System;
using System.ComponentModel;

namespace CodeIX.ServiceInjection;

/// <summary>
/// Mark a class, interface or struct to be injected by the service provider via constructor
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
[Description("Mark a class, interface or struct to be injected by the service provider via constructor")]
public class ServiceInjectionAttribute : Attribute
{
    /// <summary>
    ///  Marks a class, interface or struct to be injected by the service provider via constructor.  
    /// </summary>
    public ServiceInjectionAttribute()
    {
    }
}