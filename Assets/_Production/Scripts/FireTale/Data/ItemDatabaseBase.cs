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
        
        private class ItemEqualityComparer<T> : IEqualityComparer<T> where T : ItemBase
        {
            public bool Equals(T x, T y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null) || x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }
        
            public int GetHashCode(T obj) => obj.GetHashCode();
        }
        
        protected void Load<T>(List<T> values, List<T> targets, string targetsPath, Func<T, bool> filter) where T : ItemBase
        {
            if (!values.Any())
                return;
            
            List<Type> types = values.Select(x => x.GetType()).Distinct().ToList();
            Assert.IsTrue(types.Count == 1);
            Type type = types.First();
            
            string CreateItemPath(T item)
            {
                string itemName = $"{item.GetType().Name}_{item.Name.Replace(" ", "")}.asset";
                return AssetDatabase.GenerateUniqueAssetPath(Path.Combine(targetsPath, itemName));
            }
            
            var comparer = new ItemEqualityComparer<T>();
            Dictionary<T, string> itemPaths = targets.Where(filter)
                .ToDictionary(item => item, AssetDatabase.GetAssetPath);
            
            List<(T item, string)> itemsToAdd = values.Except(targets, comparer)
                .Select(item => (item, CreateItemPath(item))).ToList();

            List<(T item, string)> itemsToUpdate = values.Intersect(targets, comparer)
                .Select(item => (item, itemPaths[item])).ToList();
            
            List<(T item, string)> itemsToDelete = targets.Where(filter).Except(values, comparer)
                .Select(item => (item, itemPaths[item])).ToList();

            foreach ((T item, string path) in itemsToAdd)
            {
                Debug.Log("Creating " + path);
                AssetDatabase.CreateAsset(item, path);
            }

            foreach ((T item, string path) in itemsToUpdate)
            {
                Debug.Log("Updating " + path);
                T serializedItem = AssetDatabase.LoadMainAssetAtPath(path) as T;
                Assert.IsNotNull(serializedItem);
                serializedItem.UpdateData(item);
            }

            foreach ((T _, string path) in itemsToDelete)
            {
                Debug.Log("Deleting " + path);
                AssetDatabase.DeleteAsset(path);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            foreach ((T _, string path) in itemsToAdd)
                targets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
            
            foreach ((T item, string _) in itemsToDelete)
                targets.Remove(item);
        }
#endif
    }
}