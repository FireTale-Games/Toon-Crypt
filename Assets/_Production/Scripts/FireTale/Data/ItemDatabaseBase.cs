using System.Collections.Generic;
using UnityEngine;

namespace FT.Data
{
    public class ItemDatabaseBase<T, TI> : ScriptableObject where T : ItemBase where TI : ItemDatabaseBase<T, TI>
    {
        [SerializeField] protected List<T> Items = new();

        public static TI Database => _database ??= Resources.Load(typeof(TI).Name) as TI;
        private static TI _database;
    }
}