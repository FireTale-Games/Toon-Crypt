using FT.Data;
using UnityEditor;

namespace Editor.Items
{
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Item item = (Item) target;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Unique ID", item.Id.ToString());
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}