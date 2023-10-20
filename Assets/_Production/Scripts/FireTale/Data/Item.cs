using UnityEngine;

namespace FT.Data
{
    public class Item : ItemBase
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Transform Prefab { get; private set; }
        [field: SerializeField] public Texture2D Icon { get; private set; }
    }
}