using System.Collections.Generic;
using System.Linq;
using Editor.Toolkit;
using Editor.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Items
{
    public class ItemsEditor : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _rootTree;
        private EditorScreenBase _itemDataBaseEditor;
        
        [MenuItem("Toon Crypt/Items Editor")]
        public static void InitializeEditorWindow() =>
            EditorHelpers.CreateEditorWindow<ItemsEditor>("Items Editor", 400.0f, 1920.0f);

        private void CreateGUI()
        {
            EditorHelpers.GenerateWindowElement(rootVisualElement, _rootTree, ToString());
            List<VisualElement> dataFixedList = rootVisualElement.Q<VisualElement>("Content").Children().ToList();

            VisualElement detailsContent = rootVisualElement.Q<VisualElement>("DetailsContent");
            foreach (VisualElement visualElement in dataFixedList)
            {   
                visualElement.RegisterCallback<ClickEvent>(_ =>
                {
                    if (detailsContent.childCount > 0)
                        detailsContent.RemoveAt(0);
                
                    EditorHelpers.GenerateWindowElement(detailsContent, null, visualElement.name);

                    _itemDataBaseEditor = visualElement.name switch
                    {
                        nameof(ItemDatabaseEditor) => new ItemDatabaseEditor(detailsContent),
                        _ => _itemDataBaseEditor
                    };
                });
            }
        }
    }
}