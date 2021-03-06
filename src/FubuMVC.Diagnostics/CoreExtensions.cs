﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FubuMVC.Diagnostics
{
    public static class CoreExtensions
    {
        public static IEnumerable<PropertyInfo> PropertiesWhere(this Type type, Func<PropertyInfo, bool> predicate)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.Where(predicate);
        }        
    }
}