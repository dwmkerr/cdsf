using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CompositeDataServiceFramework.Server
{
    public static class PropertyInfoExtensions
    {
        public static void SetPropertyValueOnTarget(
           this PropertyInfo property,
           object target,
           object value)
        {
            property.GetSetMethod()
                    .Invoke(target, new object[] { value });
        }

        public static object GetPropertyValueFromTarget(
           this PropertyInfo property,
           object target)
        {
            return property.GetGetMethod()
                           .Invoke(target, new object[] { });
        }
    }
}
