using UnityEngine;

namespace FT.TD
{
    public class CharacterCameraController : MonoBehaviour, IViewController
    {
        [SerializeField] private Transform _rootCameraTransform;
        private Camera _camera;
        private CharacterState _characterState;

        private void Awake()
        {
            _camera = Camera.main;
            _characterState = GetComponent<Character>().State;
        }

        private void LateUpdate()
        {
            float distance = Vector3.Distance(_characterState.LookDirection.Value, transform.position);
            if (distance < 8)
            {
                _rootCameraTransform.position = transform.position;
                return;
            }
            
            Vector3 direction = (_characterState.LookDirection.Value - transform.position).normalized;
            _rootCameraTransform.position = transform.position + direction * 5;
        }
        
        public Vector3 HitLocation(Vector2 mousePosition)
        {
            Plane plane = new(Vector3.up, transform.position);
            Ray ray = _camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, 0));
            
            // Check for intersection
            if (!plane.Raycast(ray, out float enter)) 
                return Vector3.zero;

            return ray.GetPoint(enter) - transform.position;
        }
    }
}