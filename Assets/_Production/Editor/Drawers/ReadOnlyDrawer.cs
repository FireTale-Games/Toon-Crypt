using FT.Tools.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ReadOnly))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}