using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media;
using McMDK.Utils;

namespace McMDK.Plugin.UI
{
    public class StringToObjectConverter
    {

#pragma warning disable 693
        public static GuiComponents StringToComponents(string obj)
        {
            GuiComponents component;
            try
            {
                component = (GuiComponents) Enum.Parse(typeof (GuiComponents), obj);
            }
            catch (Exception)
            {
                component = GuiComponents.Null;
            }
            return component;
        }

        public static Type StringTo<Type>(string obj) where Type : struct
        {
            if (!String.IsNullOrEmpty(obj))
            {
                var converter = TypeDescriptor.GetConverter(typeof(Type));
                var convertFromString = converter.ConvertFromString(obj);
                if (convertFromString != null) return (Type)convertFromString;
            }
            return default(Type);
        }

        public static Type StringTo<Type>(string obj, object def) where Type : struct
        {
            if (!String.IsNullOrEmpty(obj))
            {
                var converter = TypeDescriptor.GetConverter(typeof (Type));
                var convertFromString = converter.ConvertFromString(obj);
                if (convertFromString != null) return (Type) convertFromString;
            }
            return (Type) def;
        }

        public static object StringToEnum(string obj, Type type)
        {
            return StringToEnum(obj, type, null);
        }

        public static object StringToEnum(string obj, Type type, object def)
        {
            try
            {
                return Enum.Parse(type, obj);
            }
            catch (Exception)
            {
                if (!String.IsNullOrEmpty(obj))
                {
                    Define.GetLogger().Error(def + " cannot convert to " + type + ".");
                }
                return def;
            }
        }

        public static object StringToProperty(string obj, Type type)
        {
            return StringToProperty(obj, type, null);
        }

        public static object StringToProperty(string obj, Type type, object def)
        {
            try
            {
                PropertyInfo info = type.GetProperty(obj);
                return info.GetValue(null);
            }
            catch (Exception)
            {
                return def;
            }
        }

        public static Brush StringToBrush(string obj)
        {
            return StringToBrush(obj, null);
        }

        public static Brush StringToBrush(string obj, Brush def)
        {
            try
            {
                if (String.IsNullOrEmpty(obj))
                {
                    return def;
                }
                if (obj.StartsWith("#"))
                {
                    Color color = new Color();
                    if (obj.Length == 9)
                    {
                        color.A = (byte) Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.R = (byte) Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.G = (byte) Convert.ToInt32(obj.Substring(5, 2), 16);
                        color.B = (byte) Convert.ToInt32(obj.Substring(7, 2), 16);
                    }
                    else if (obj.Length == 7)
                    {
                        color.R = (byte) Convert.ToInt32(obj.Substring(1, 2), 16);
                        color.G = (byte) Convert.ToInt32(obj.Substring(3, 2), 16);
                        color.B = (byte) Convert.ToInt32(obj.Substring(5, 2), 16);
                    }
                    else
                    {
                        color.R = (byte) Convert.ToInt32("00", 16);
                        color.G = (byte) Convert.ToInt32("00", 16);
                        color.B = (byte) Convert.ToInt32("00", 16);
                    }
                    return new SolidColorBrush(color);
                }
                PropertyInfo info = typeof (Brushes).GetProperty(obj);
                return (Brush) info.GetValue(null);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
            return def;
        }
    }
}
