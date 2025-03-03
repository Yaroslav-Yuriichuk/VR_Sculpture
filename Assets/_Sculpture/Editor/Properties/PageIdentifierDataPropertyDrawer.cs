using System;
using System.Collections.Generic;
using System.Linq;
using _Sculpture.Runtime.UI;
using UnityEditor;
using UnityEngine;

namespace _Sculpture.Editor.Properties
{
    [CustomPropertyDrawer(typeof(PageIdentifierData))]
    internal sealed class PageIdentifierDataPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty enumTypeNameProperty = property.FindPropertyRelative("<EnumTypeName>k__BackingField");
            SerializedProperty enumAssemblyNameProperty = property.FindPropertyRelative("<EnumAssemblyName>k__BackingField");
            SerializedProperty enumValueNameProperty = property.FindPropertyRelative("<EnumValueName>k__BackingField");

            position.height = EditorGUIUtility.singleLineHeight;

            TypeCache.TypeCollection pageIdentifiers = TypeCache.GetTypesWithAttribute<PageIdentifiersAttribute>();

            if (pageIdentifiers.Count == 0)
            {
                Debug.LogError("No page identifiers found, please fill up enums.");
                return;
            }

            List<Enum> pageValues = new List<Enum>();

            foreach (Type type in pageIdentifiers)
            {
                foreach (Enum pageValue in Enum.GetValues(type))
                {
                    pageValues.Add(pageValue);
                }
            }

            if (string.IsNullOrEmpty(enumTypeNameProperty.stringValue) ||
                string.IsNullOrEmpty(enumAssemblyNameProperty.stringValue) ||
                string.IsNullOrEmpty(enumValueNameProperty.stringValue))
            {
                enumTypeNameProperty.stringValue = pageValues[0].GetType().FullName;
                enumAssemblyNameProperty.stringValue = pageValues[0].GetType().Assembly.FullName;
                enumValueNameProperty.stringValue = pageValues[0].ToString();
            }

            int selectedIndex = -1;

            for (int i = 0; i < pageValues.Count; i++)
            {
                Enum pageValue = pageValues[i];

                if (pageValue.GetType().FullName == enumTypeNameProperty.stringValue &&
                    pageValue.GetType().Assembly.FullName == enumAssemblyNameProperty.stringValue &&
                    pageValue.ToString() == enumValueNameProperty.stringValue)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (selectedIndex < 0)
            {
                enumTypeNameProperty.stringValue = pageValues[0].GetType().FullName;
                enumAssemblyNameProperty.stringValue = pageValues[0].GetType().Assembly.FullName;
                enumValueNameProperty.stringValue = pageValues[0].ToString();

                selectedIndex = 0;
            }

            string[] options = pageValues.Select(v => $"{v.GetType().Name}.{v.ToString()}").ToArray();

            EditorGUI.LabelField(position, "Type");
            int index = EditorGUI.Popup(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width, position.height), selectedIndex, options);

            if (index != selectedIndex)
            {
                enumTypeNameProperty.stringValue = pageValues[index].GetType().FullName;
                enumAssemblyNameProperty.stringValue = pageValues[index].GetType().Assembly.FullName;
                enumValueNameProperty.stringValue = pageValues[index].ToString();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}