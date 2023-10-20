using UnityEngine;

namespace FT.Data
{
    public enum AbilityHitType : byte { SINGLE_TARGET, AOE }
    
    [CreateAssetMenu(fileName = "Ability", menuName = "FireTale/Items/Ability")]
    public class Ability : Item
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public AbilityHitType HitType { get; private set; }
    }
}