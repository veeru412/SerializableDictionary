using UnityEditor;
using UnityEngine;

namespace VB.SerializeDictionary
{
  [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
  public class SerializableDictionaryDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      // Draw foldout
      property.isExpanded = EditorGUI.Foldout(
          new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
          property.isExpanded, label);

      if (!property.isExpanded) return;

      EditorGUI.indentLevel++;

      SerializedProperty keys = property.FindPropertyRelative("keys");
      SerializedProperty values = property.FindPropertyRelative("values");

      int count = Mathf.Min(keys.arraySize, values.arraySize);

      float lineHeight = EditorGUIUtility.singleLineHeight + 2;
      Rect line = new Rect(position.x, position.y + lineHeight, position.width, EditorGUIUtility.singleLineHeight);

      for (int i = 0; i < count; i++)
      {
        float halfWidth = (line.width - 30f) / 2f; // leave space for X button

        SerializedProperty keyProp = keys.GetArrayElementAtIndex(i);
        SerializedProperty valueProp = values.GetArrayElementAtIndex(i);

        // Draw Key
        Rect keyRect = new Rect(line.x, line.y, halfWidth - 2, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none, true);

        // Draw Value
        Rect valueRect = new Rect(line.x + halfWidth + 2, line.y, halfWidth - 2, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none, true);

        Rect removeRect = new Rect(line.x + halfWidth * 2 + 4, line.y, 26, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(removeRect, "X"))
        {
          keys.DeleteArrayElementAtIndex(i);
          values.DeleteArrayElementAtIndex(i);
          property.serializedObject.ApplyModifiedProperties();
          EditorUtility.SetDirty(property.serializedObject.targetObject);
          break; 
        }

        line.y += lineHeight;
      }

      Rect buttonRect = new Rect(position.x, line.y, position.width, EditorGUIUtility.singleLineHeight);
      if (GUI.Button(buttonRect, "Add Entry"))
      {
        keys.arraySize++;
        values.arraySize++;

        SerializedProperty newKey = keys.GetArrayElementAtIndex(keys.arraySize - 1);
        SerializedProperty newValue = values.GetArrayElementAtIndex(values.arraySize - 1);

        InitializeProperty(newKey);
        InitializeProperty(newValue);

        property.serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(property.serializedObject.targetObject);
      }

      EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;

      SerializedProperty keys = property.FindPropertyRelative("keys");
      int count = keys.arraySize;

      float lineHeight = EditorGUIUtility.singleLineHeight + 2;
      return EditorGUIUtility.singleLineHeight + (count + 1) * lineHeight;
    }

    private void InitializeProperty(SerializedProperty prop)
    {
      switch (prop.propertyType)
      {
        case SerializedPropertyType.String:
          prop.stringValue = "";
          break;
        case SerializedPropertyType.Integer:
          prop.intValue = 0;
          break;
        case SerializedPropertyType.Boolean:
          prop.boolValue = false;
          break;
        case SerializedPropertyType.Float:
          prop.floatValue = 0f;
          break;
        case SerializedPropertyType.ObjectReference:
          prop.objectReferenceValue = null;
          break;
      }
    }
  }
}
