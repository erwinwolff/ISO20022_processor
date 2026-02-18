using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace System
{
    public static class XmlDefinitionInflator
    {
        private static bool IsPropertyACollection(PropertyInfo property)
        {
            return typeof(Array).IsAssignableFrom(property.PropertyType);
        }

        public static void InflateXmlPocoDefinition(this object obj)
        {
            if (obj == null)
                return;

            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (!property.CanWrite)
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

                    if (listPropertyType == null)
                        continue;

                    ConstructorInfo[] info = listPropertyType
                        .GetConstructors()
                        .Where(x => x.IsPublic && 
                                    x.GetParameters().Count() == 0)
                        .ToArray();

                    Array collectionInstance;
                    bool hasAssignedValue = false;

                    if (info.Any())
                    {
                        collectionInstance = Array.CreateInstance(listPropertyType, 1);

                        object createdType = null;

                        if (listPropertyType == typeof(bool))
                        {
                            createdType = false;
                            hasAssignedValue = true;
                        }

                        if (listPropertyType == typeof(string))
                        {
                            createdType = property.Name;
                            hasAssignedValue = true;
                        }

                        if (listPropertyType == typeof(decimal))
                        {
                            createdType = 99M;
                            hasAssignedValue = true;
                        }

                        if (listPropertyType == typeof(int))
                        {
                            createdType = 1;
                            hasAssignedValue = true;
                        }

                        if (listPropertyType.IsEnum)
                        {
                            var enumValues = Enum.GetValues(listPropertyType);
                            if (enumValues.Length > 0)
                            {
                                createdType = enumValues.GetValue(0);
                            }
                            hasAssignedValue = true;
                        }

                        if (hasAssignedValue == false && 
                            listPropertyType != typeof(object))
                        {
                            createdType = Activator.CreateInstance(listPropertyType);
                            createdType.InflateXmlPocoDefinition();
                            hasAssignedValue = true;
                        }

                        if (hasAssignedValue == false &&
                           listPropertyType == typeof(object))
                        {
                            var xmlElementAttributes = property.GetCustomAttributes()
                                .Where(attr => attr.GetType() == typeof(XmlElementAttribute))
                                .Cast<XmlElementAttribute>()
                                .ToArray();

                            if (xmlElementAttributes.Any())
                            {
                                var suggestedType = xmlElementAttributes.First().Type;

                                if (suggestedType != typeof(string))
                                {
                                    createdType = Activator.CreateInstance(suggestedType);
                                    createdType.InflateXmlPocoDefinition();
                                }
                                else
                                    createdType = property.Name;

                                 hasAssignedValue = true;
                            }
                        }

                        if (createdType != null)
                            collectionInstance.SetValue(createdType, 0);
                    }
                    else if (listPropertyType.IsEnum)
                    {
                        var enumValues = Enum.GetValues(listPropertyType);
                        collectionInstance = Array.CreateInstance(listPropertyType, 1);
                        collectionInstance.SetValue(enumValues.GetValue(0), 0);

                        hasAssignedValue = true;
                    }
                    else
                    {
                        collectionInstance = Array.CreateInstance(listPropertyType, 0);
                    }

                    if (hasAssignedValue)
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
                    constructor.GetParameters().Length == 0)
                {
                    if (property.PropertyType != typeof(object))
                    {
                        if (property.CanWrite)
                            property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        var xmlElementAttributes = property.GetCustomAttributes()
                            .Where(attr => attr.GetType() == typeof(XmlElementAttribute))
                            .Cast<XmlElementAttribute>()
                            .ToArray();
                        if (xmlElementAttributes.Any() &&
                            property.CanWrite)
                        {
                            var suggestedType = xmlElementAttributes.First().Type;

                            if (suggestedType != typeof(string))
                                property.SetValue(obj, Activator.CreateInstance(suggestedType));
                            else
                                property.SetValue(obj, property.Name);
                        }
                    }
                    var value = property.GetValue(obj);

                    InflateXmlPocoDefinition(value);
                }
            }
        }
    }
}