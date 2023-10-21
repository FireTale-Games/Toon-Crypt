using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FT.Tools.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace FT.Data
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "FireTale/ItemDatabase")]
    public class ItemDatabase : ItemDatabaseBase<Item, ItemDatabase>
    {
#if UNITY_EDITOR
         public void UpdateData(DataTable csvFile)
        {
            foreach (DataRow dataRow in csvFile.Rows)
            {
                int id = ItemBase.NameToId(dataRow.Parse<string>("Name"));
                Get(id)?.Setup(dataRow);
            }
        }
         
        public void Load(DataTable csvFile, Type type)
        {
            string indexPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            Assert.IsFalse(string.IsNullOrEmpty(indexPath));

            string allItemsPath = Path.Combine(indexPath, "Items");
            string itemsPath = Path.Combine(allItemsPath, type.Name);
            Debug.Log("Target path " + itemsPath);
            
            if (!AssetDatabase.IsValidFolder(itemsPath))
            {
                Debug.Log($"Creating folder {itemsPath}");
                AssetDatabase.CreateFolder(allItemsPath, type.Name);
            }
            
            List<Item> items = new();

            foreach (DataRow dataRow in csvFile.Rows)
                items.AddIfNotNull(CreateItem(dataRow, type));
            
            Load(items, _items, itemsPath, x => x.GetType() == type);
        }
        
        private static Item CreateItem(DataRow dataRow, Type type)
        {
            Item scriptableObject = CreateInstance(type) as Item;
            if (scriptableObject == null) 
                return null;
            
            scriptableObject.Setup(dataRow);
            return scriptableObject;
        }

        public static List<Type> GetAllItemTypes() =>
            typeof(Item).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Item))).ToList();

        public static Type GetType(string typeName) => GetAllItemTypes().FirstOrDefault(t => t.Name == typeName);
    }
#endif
}