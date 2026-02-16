using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

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
                bool hasTopLevelXmlIgnoreAttribute = property.PropertyType.GetCustomAttributes()
                        .Any(attr => attr.GetType() == typeof(XmlIgnoreAttribute));

                if (hasTopLevelXmlIgnoreAttribute)
                    continue;

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

                    bool hasXmlIgnoreAttribute = listPropertyType.GetCustomAttributes()
                        .Any(attr => attr.GetType() == typeof(XmlIgnoreAttribute));

                    ConstructorInfo[] info = listPropertyType
                        .GetConstructors()
                        .Where(x => x.IsPublic && 
                                    x.GetParameters().Count() == 0)
                        .ToArray();

                    Array collectionInstance;
                    bool assignedValue = false;

                    if (info.Any() && !hasXmlIgnoreAttribute)
                    {
                        collectionInstance = Array.CreateInstance(listPropertyType, 1);

                        object createdType = null;

                        if (listPropertyType == typeof(bool))
                        {
                            createdType = false;
                            assignedValue = true;
                        }

                        if (listPropertyType == typeof(string))
                        {
                            createdType = property.Name;
                            assignedValue = true;
                        }

                        if (listPropertyType == typeof(decimal))
                        {
                            createdType = 99M;
                            assignedValue = true;
                        }

                        if (listPropertyType == typeof(int))
                        {
                            createdType = 1;
                            assignedValue = true;
                        }

                        if (listPropertyType.IsEnum)
                        {
                            var enumValues = Enum.GetValues(listPropertyType);
                            if (enumValues.Length > 0)
                            {
                                createdType = enumValues.GetValue(0);
                            }
                            assignedValue = true;
                        }

                        if (assignedValue == false && 
                            listPropertyType != typeof(object))
                        {
                            createdType = Activator.CreateInstance(listPropertyType);
                            createdType.TraverseAndInitAllProperties();
                        }

                        if (createdType != null)
                            collectionInstance.SetValue(createdType, 0);
                    }
                    else
                    {
                        collectionInstance = Array.CreateInstance(listPropertyType, 0);
                    }

                    if (assignedValue)
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