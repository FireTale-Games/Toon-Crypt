using UnityEngine;

namespace FT.Data
{
    [CreateAssetMenu(fileName = "Ability", menuName = "FireTale/Items/Ability")]
    public class Ability : Item
    {
        [field: SerializeField] public float Damage { get; private set; }
    }
}