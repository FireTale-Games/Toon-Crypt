using FT.Inputs;
using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent, RequireComponent(typeof(PlayerInputController), typeof(CharacterBehaviourController))]
    public class CharacterBehaviourController : MonoBehaviour
    {
        private CharacterState _state;
        
        private void Awake()
        {
            GetComponent<PlayerInputController>()?.OnInput.AddObserver(OnInput);
            _state = GetComponent<Character>()?.State;
        }

        private void OnInput(InputData inputData)
        {
            _state.MoveDirection.Set(new Vector3(inputData.moveDirection.x, 0, inputData.moveDirection.y));
            _state.IsShooting.Set(inputData.isShooting);
        }
    }
}