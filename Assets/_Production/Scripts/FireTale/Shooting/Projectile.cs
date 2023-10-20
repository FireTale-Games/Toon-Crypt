using System;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.Shooting
{
    public class Projectile : MonoBehaviour
    {
        public IObservableAction<Action> OnHit => _onHit;
        private readonly ObservableAction<Action> _onHit = new();
        
        
        private void FixedUpdate() => 
            transform.position += transform.forward * (15.0f * Time.deltaTime);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHit _) == false)
                return;
            
            _onHit.Action?.Invoke();
            Destroy(gameObject);
        }
    }
}