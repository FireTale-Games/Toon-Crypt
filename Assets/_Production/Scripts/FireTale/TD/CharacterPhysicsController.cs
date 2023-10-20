using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    public class CharacterPhysicsController : Character
    {
        private CharacterController _controller;

        private void Awake() => _controller = GetComponent<CharacterController>();

        private void FixedUpdate() => 
            _controller.Move(State.MoveDirection.Value * (Time.deltaTime * Parameters.WalkSpeed));
    }
}