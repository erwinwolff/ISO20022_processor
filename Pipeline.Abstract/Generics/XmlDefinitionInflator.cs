using System.Collections;
using System.Reflection;

namespace System
{
    public static class XmlDefinitionInflator
    {
        public static bool IsPropertyACollection(PropertyInfo property)
        {
            return (!typeof(string).Equals(property.PropertyType) &&
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType));
        }

        public static void TraverseAndInitAllProperties(this object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(obj, false);
                    continue;
                }

                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, property.Name);
                    continue;
                }

                if (property.PropertyType == typeof(decimal))
                {
                    property.SetValue(obj, 99M);
                    continue;
                }

                if (property.PropertyType == typeof(int))
                {
                    property.SetValue(obj, 1);
                    continue;
                }

                if (IsPropertyACollection(property))
                {
                    var listPropertyType = property.PropertyType.GetElementType();

                    if (listPropertyType == null) continue;

                    var collectionInstance = Array.CreateInstance(listPropertyType, 0);
                    property.SetValue(obj, collectionInstance);
                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    var enumValues = Enum.GetValues(property.PropertyType);
                    if (enumValues.Length > 0)
                    {
                        property.SetValue(obj, enumValues.GetValue(0));
                    }
                    continue;
                }

                var constructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
                if (constructor != null &&
                    constructor.GetParameters().Length == 0 &&
                    property.PropertyType != typeof(object)) // xml validator crashes if we try to create an instance of object type, so we skip it
                {
                    property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    var value = property.GetValue(obj);

                    Console.WriteLine($"{property.Name}: {value}");

                    TraverseAndInitAllProperties(value);
                }
            }
        }
    }
}