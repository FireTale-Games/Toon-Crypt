using System.Collections.Generic;
using UnityEngine;

namespace FT.Data
{
    public class ItemDatabaseBase<T, TI> : ScriptableObject where T : ItemBase where TI : ItemDatabaseBase<T, TI>
    {
        [SerializeField] protected List<T> _items = new();

        public static TI Database => _database ??= Resources.Load(typeof(TI).Name) as TI;
        private static TI _database;
        
        public static T Get(int id) => Database._items.Find(item => item.Id == id);
        public static TT Get<TT>(int id) where TT : T => Get(id) as TT;
    }
}