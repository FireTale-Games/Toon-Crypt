using FT.Inputs;
using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent, RequireComponent(typeof(PlayerInputController), typeof(CharacterPhysicsController))]
    public class CharacterBehaviourController : MonoBehaviour
    {
        private CharacterPhysicsController _physicsController;
        private CharacterState _state;
        
        private void Awake()
        {
            GetComponent<PlayerInputController>()?.OnInput.AddObserver(OnInput);
            _physicsController = GetComponent<CharacterPhysicsController>();
            _state = GetComponent<Character>()?.State;
        }

        private void OnInput(InputData inputData)
        {
            _state.IsInventory.Set(inputData.isInventory);
            _state.IsEscape.Set(inputData.isEscape);
            _state.IsShooting.Set(inputData.isShooting);
            _physicsController.SetMoveAndMouseValues(inputData.moveDirection, inputData.mousePosition);
        }
    }
}