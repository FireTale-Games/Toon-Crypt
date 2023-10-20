using UnityEngine;

namespace FT.TD
{
    public class Character : MonoBehaviour
    {
        public CharacterState State => _state ??= new CharacterState();
        private CharacterState _state;

        protected CharacterParameters Parameters => _parameters ??= GetComponent<CharacterParameters>();
        private CharacterParameters _parameters;
    }
}