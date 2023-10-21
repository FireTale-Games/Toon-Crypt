using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace FT.Data
{
    public class ItemDatabaseBase<T, TI> : ScriptableObject where T : ItemBase where TI : ItemDatabaseBase<T, TI>
    {
        [SerializeField] private string _spreadsheetId;
        [SerializeField] protected List<T> _items = new();

        public static TI Database => _database ??= Resources.Load(typeof(TI).Name) as TI;
        private static TI _database;
        
        public static T Get(int id) => Database._items.Find(item => item.Id == id);
        public static TT Get<TT>(int id) where TT : T => Get(id) as TT;
        
#if UNITY_EDITOR
        public string GetDownloadUrl(Type type) => 
            $"https://docs.google.com/spreadsheets/d/{_spreadsheetId}/gviz/tq?tqx=out:csv&sheet={type.Name}";
        
        private class ItemEqualityComparer<TT> : IEqualityComparer<TT> where TT : ItemBase
        {
            public bool Equals(TT x, TT y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null) || x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }
        
            public int GetHashCode(TT obj) => obj.GetHashCode();
        }
        
        protected void Load<TT>(List<TT> values, List<TT> targets, string targetsPath, Func<TT, bool> filter) where TT : ItemBase
        {
            if (!values.Any())
                return;
            
            List<Type> types = values.Select(x => x.GetType()).Distinct().ToList();
            Assert.IsTrue(types.Count == 1);

            string CreateItemPath(TT item)
            {
                string itemName = $"{item.GetType().Name}_{item.Name.Replace(" ", "")}.asset";
                return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(targetsPath, itemName));
            }
            
            ItemEqualityComparer<TT> comparer = new();
            Dictionary<TT, string> itemPaths = targets.Where(filter)
                .ToDictionary(item => item, AssetDatabase.GetAssetPath);
            
            List<(TT item, string)> itemsToAdd = values.Except(targets, comparer)
                .Select(item => (item, CreateItemPath(item))).ToList();

            List<(TT item, string)> itemsToUpdate = values.Intersect(targets, comparer)
                .Select(item => (item, itemPaths[item])).ToList();
            
            List<(TT item, string)> itemsToDelete = targets.Where(filter).Except(values, comparer)
                .Select(item => (item, itemPaths[item])).ToList();

            foreach ((TT item, string path) in itemsToAdd)
            {
                Debug.Log("Creating " + path);
                AssetDatabase.CreateAsset(item, path);
            }

            foreach ((TT item, string path) in itemsToUpdate)
            {
                Debug.Log("Updating " + path);
                TT serializedItem = AssetDatabase.LoadMainAssetAtPath(path) as TT;
                Assert.IsNotNull(serializedItem);
                serializedItem.UpdateData(item);
            }

            foreach ((TT _, string path) in itemsToDelete)
            {
                Debug.Log("Deleting " + path);
                AssetDatabase.DeleteAsset(path);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            foreach ((TT _, string path) in itemsToAdd)
                targets.Add(AssetDatabase.LoadAssetAtPath<TT>(path));
            
            foreach ((TT item, string _) in itemsToDelete)
                targets.Remove(item);
        }
#endif
    }
}