using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public class Weapon : Item
    {
        [field: SerializeField] public float CriticalChance { get; private set; }
        [field: SerializeField] public float CriticalDamage { get; private set; }
        
        
#if UNITY_EDITOR
        public override void Setup(DataRow data)
        {
            base.Setup(data);

            CriticalChance = data.Parse<float>(nameof(CriticalChance));
            CriticalDamage = data.Parse<float>(nameof(CriticalDamage));
        }
#endif
    }
}