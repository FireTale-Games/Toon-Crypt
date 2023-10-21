using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public class Item : ItemBase
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Transform Prefab { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
     
#if UNITY_EDITOR
        public virtual void Setup(DataRow data)
        {
            Name = data.Parse<string>(nameof(Name));
            DisplayName = data.Parse<string>(nameof(DisplayName));
            string PrefabName = data.Parse<string>(nameof(PrefabName));
            Prefab = Resources.Load<Transform>($"{GetType().Name}/Prefabs/{PrefabName}");
            string SpriteName = data.Parse<string>(nameof(SpriteName));
            Sprite = Resources.Load<Sprite>($"{GetType().Name}/Icons/{SpriteName}");

            name = $"{GetType().Name}_{Name}";
        }
#endif
    }
}