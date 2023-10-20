using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent]
    public class CharacterParameters : MonoBehaviour
    {
        [field: SerializeField] public float WalkSpeed { get; private set; } = 5.0f;
    }
}