using System;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.Shooting
{
    public class Projectile : MonoBehaviour
    {
        public IObservableAction<Action<IHit>> OnHit => _onHit;
        private readonly ObservableAction<Action<IHit>> _onHit = new();
        
        
        private void FixedUpdate() => 
            transform.position += transform.forward * (15.0f * Time.deltaTime);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHit hit) == false)
                return;
            
            _onHit.Action?.Invoke(hit);
            Destroy(gameObject);
        }
    }
}