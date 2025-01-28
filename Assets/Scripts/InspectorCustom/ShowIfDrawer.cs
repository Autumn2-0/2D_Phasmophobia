using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the attribute data
        ShowIfAttribute showIfAttribute = (ShowIfAttribute)attribute;

        // Find the condition property
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIfAttribute.ConditionalSourceField);

        // Check if the condition is met
        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            if (conditionProperty.boolValue)
            {
                // Draw the property if the condition is true
                EditorGUI.PropertyField(position, property, label);
            }
        }
        else
        {
            Debug.LogWarning($"Conditional field '{showIfAttribute.ConditionalSourceField}' not found or is not a boolean!");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Get the attribute data
        ShowIfAttribute showIfAttribute = (ShowIfAttribute)attribute;

        // Find the condition property
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIfAttribute.ConditionalSourceField);

        // Only take up space if the condition is true
        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean && conditionProperty.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        // Otherwise, take no space
        return 0f;
    }
}
