using System;
using System.Collections;
using UnityEngine;

namespace FT.TD
{
    public class Character : MonoBehaviour
    {
        public static Action<CharacterState> OnCharacterInitialized;
        
        public CharacterState State => _state ??= new CharacterState();
        private CharacterState _state;

        protected CharacterParameters Parameters => _parameters ??= GetComponent<CharacterParameters>();
        private CharacterParameters _parameters;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            OnCharacterInitialized?.Invoke(State);
        }
    }
}