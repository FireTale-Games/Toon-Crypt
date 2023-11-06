using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    public class CharacterPhysicsController : Character
    {
        [SerializeField] private Transform _mesh;
        private CharacterController _controller;
        
        private Vector3 _moveDirection;
        
        public void SetMoveAndMouseValues(Vector2 moveDirection) => 
            _moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);

        private void Awake() => 
            _controller = GetComponent<CharacterController>();

        private void FixedUpdate()
        {
            _controller.Move(_moveDirection * (Time.deltaTime * Parameters.WalkSpeed));
            _mesh.rotation = Quaternion.LookRotation(State.LookDirection.Value, Vector3.up);
            _mesh.rotation = Quaternion.Euler(0, _mesh.rotation.eulerAngles.y, 0);
        }
    }
}