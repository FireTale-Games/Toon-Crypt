using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Editor.Items;
using FT.Data;
using FT.Tools.CSV;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Toolkit
{
    public class ItemDatabaseEditor : EditorScreenBase
    {
        private ItemDatabase _itemDatabase;
        private List<Type> _itemTypes;
        private readonly List<(DataTable, Type)> _loadedTables = new();
        private DatasheetLoaderEditor _datasheetLoaderEditor;
    
        private int _choiceIndex;
        private Type SelectedType => _itemTypes[_choiceIndex];
    
        public ItemDatabaseEditor(VisualElement parentElement) => BindData(parentElement);
    
        protected sealed override void BindData(VisualElement parentElement)
        {
            _itemDatabase = Resources.Load(nameof(ItemDatabase)) as ItemDatabase;
            _datasheetLoaderEditor = new DatasheetLoaderEditor(_itemDatabase);
    
            SetUpButtons(parentElement);
            SetUpItemTypeDropdown(parentElement);
        }
    
        private void SetUpButtons(VisualElement parentElement)
        {
            parentElement.Q<Button>("DownloadAllCSV").clicked += OnDownloadAllCSVClicked;
            parentElement.Q<Button>("DownloadSingleCSV").clicked += OnDownloadSingleCSVClicked;
        }
    
        private void SetUpItemTypeDropdown(VisualElement parentElement)
        {
            _itemTypes ??= ItemDatabase.GetAllItemTypes();
            DropdownField selectItemType = parentElement.Q<DropdownField>("SelectedType");
            selectItemType.choices = _itemTypes.Select(x => x.Name).ToList();
            _choiceIndex = selectItemType.index = 0;
            selectItemType.RegisterValueChangedCallback(evt =>
                _choiceIndex = selectItemType.index = selectItemType.choices.IndexOf(evt.newValue));
        }
    
        private void OnDownloadSingleCSVClicked()
        {
            _loadedTables.Clear();
            
            (string, Action<byte[]>) request = CreateRequest(SelectedType);
            _datasheetLoaderEditor.DownloadSingle(request, _loadedTables);
        }
    
        private void OnDownloadAllCSVClicked()
        {
            _loadedTables.Clear();
            
            List<(string, Action<byte[]>)> requests = _itemTypes.Select(CreateRequest).ToList();
            _datasheetLoaderEditor.DownloadMultiple(requests, _loadedTables);
        }
        
        private void OnData(byte[] rawData, Type type)
        {
            DataTable table = CSVReader.ReadCSVData(rawData, true);
            table.TableName = type.Name;
            _loadedTables.Add((table, type));
        }
    
        private (string, Action<byte[]>) CreateRequest(Type type)
        {
            void Data(byte[] rawData) => OnData(rawData, type);
            return (_itemDatabase.GetDownloadUrl(type), Data);
        }
    }
}