using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Helpers
{
    public static class EditorHelpers
    {
        public static void CreateEditorWindow<T>(string name, float minSize = 400.0f, float maxSize = 1200.0f) where T : EditorWindow
        {
            T _editor = EditorWindow.GetWindow<T>(name);

            _editor.minSize = new Vector2(minSize, minSize);
            _editor.maxSize = new Vector2(maxSize, maxSize);
        }

        public static void GenerateWindowElement(VisualElement rootElement, VisualTreeAsset rootTree, string rootName)
        {
            if (rootTree != null)
            {
                rootElement.Add(rootTree.Instantiate());
                return;
            }

            string assetName = $"{rootName.TrimStart().Trim('(', ')').Split('.').LastOrDefault()}_UXML";
            string[] guids = AssetDatabase.FindAssets(assetName);
            switch (guids.Length)
            {
                case 0:
                    Debug.LogError(assetName + " asset has not been found");
                    return;
                case > 1:
                    Debug.LogError(assetName + " has more than 1 asset");
                    return;
            }
            
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            rootElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath).Instantiate());
        }
    }
}
