using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public enum AbilitySpreadType : byte { SINGLE_TARGET, MULTIPLE_TARGETS }
    
    [CreateAssetMenu(fileName = "Ability", menuName = "FireTale/Items/Ability")]
    public class Ability : Item
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public AbilitySpreadType SpreadType { get; private set; }

#if UNITY_EDITOR
        public override void Setup(DataRow data)
        {
            base.Setup(data);

            Damage = data.Parse<float>(nameof(Damage));
            SpreadType = data.Parse<AbilitySpreadType>(nameof(SpreadType));
        }
#endif
    }
}