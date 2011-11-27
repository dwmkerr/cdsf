using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompositeDataServiceFramework.Server
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type generic)
        {
            while (type != typeof(object) && type != null)
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (cur.IsGenericType && generic.GetGenericTypeDefinition() == cur.GetGenericTypeDefinition())
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }
    }
}
