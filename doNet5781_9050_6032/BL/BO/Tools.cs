﻿using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public static class Tools
    {
        public static string ToStringProperty<T>(this T t, string suffix = "")
        {
            string str = "";
            foreach (PropertyInfo prop in t.GetType().GetProperties())
            {
                var value = prop.GetValue(t, null);
                if (value is string)
                    str += "     " + suffix + prop.Name + ": " + value;
                else if (value is IEnumerable )
                    foreach (var item in (IEnumerable)value)
                        str += item.ToStringProperty("   ");
                else
                    str += "     " + suffix + prop.Name + ": " + value;
            }
            return str;
        }
    }
}
