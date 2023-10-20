using UnityEngine;

namespace FT.TD
{
    [DisallowMultipleComponent, RequireComponent(typeof(CharacterController))]
    public class CharacterPhysicsController : Character
    {
        [SerializeField] private Transform _mesh;
        private Camera _camera;
        private CharacterController _controller;
        private Transform _meshDirection;
        
        private Vector3 _moveDirection;
        private Vector2 _mouseDirection;
        
        public void SetMoveAndMouseValues(Vector2 moveDirection, Vector2 mouseDirection)
        {
            _moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
            _mouseDirection = mouseDirection;
        }
        
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _camera = Camera.main;
            _meshDirection = _mesh.GetChild(0);
        }

        private void FixedUpdate()
        {
            _controller.Move(_moveDirection * (Time.deltaTime * Parameters.WalkSpeed));
            RotateMesh();
        }

        private void RotateMesh()
        {
            Plane plane = new(Vector3.up, _meshDirection.position);
            Ray ray = _camera.ScreenPointToRay(new Vector3(_mouseDirection.x, _mouseDirection.y, 0));
            
            // Check for intersection
            if (!plane.Raycast(ray, out float enter)) 
                return;

            Vector3 directionToHitPoint = ray.GetPoint(enter) - _mesh.position;
            _mesh.rotation = Quaternion.LookRotation(directionToHitPoint, Vector3.up);
            _mesh.rotation = Quaternion.Euler(0, _mesh.rotation.eulerAngles.y, 0);
        }
    }
}