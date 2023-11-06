using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FT.Data
{
    public class ItemBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; protected set; }
        
        public int Id => GetHashCode();
        
        public override int GetHashCode() => NameToId(Name);
        
        public override bool Equals(object other) => 
            other is ItemBase identifier && identifier.Id == Id;
        
        public static int NameToId(string input)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            int uniqueID = BitConverter.ToInt32(bytes, 0);
            return uniqueID;
        }
                
#if UNITY_EDITOR
        public void UpdateData(ItemBase item)
        {
            item.name = name;
            EditorUtility.CopySerialized(item, this);
        }
#endif
    }
}