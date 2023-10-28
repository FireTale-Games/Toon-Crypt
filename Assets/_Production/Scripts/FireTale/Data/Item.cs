using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public enum Rarity : byte {None, Common, Uncommon, Rare, Epic, Legendary}
    
    public class Item : ItemBase
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Transform Prefab { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public Rarity Rarity { get; private set; }
     
#if UNITY_EDITOR
        public virtual void Setup(DataRow data)
        {
            Name = data.Parse<string>(nameof(Name));
            DisplayName = data.Parse<string>(nameof(DisplayName));
            string PrefabName = data.Parse<string>(nameof(PrefabName));
            Prefab = Resources.Load<Transform>($"{GetType().Name}/Prefabs/{PrefabName}");
            string SpriteName = data.Parse<string>(nameof(SpriteName));
            Sprite = Resources.Load<Sprite>($"{GetType().Name}/Icons/{SpriteName}");
            Rarity = data.Parse<Rarity>(nameof(Rarity));

            name = $"{GetType().Name}_{Name}";
        }
#endif
    }
}