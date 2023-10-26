#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

using UnityEngine;

namespace FT.Data
{
    public enum Rarity {Common, Uncommon, Rare, Epic, Legendary}
    
    public class Weapon : Item
    {
        [field: SerializeField] public Rarity Rarity { get; private set; }

#if UNITY_EDITOR
        public override void Setup(DataRow data)
        {
            base.Setup(data);

            Rarity = data.Parse<Rarity>(nameof(Rarity));
        }
    }
#endif
}