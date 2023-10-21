using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace FT.Data
{
    public class ItemBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        
        public int Id => NameToId(Name);
        
        public static int NameToId(string input)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            int uniqueID = BitConverter.ToInt32(bytes, 0);
            return uniqueID;
        }
    }
}