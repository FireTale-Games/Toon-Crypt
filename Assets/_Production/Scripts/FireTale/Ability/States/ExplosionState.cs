using System.Collections.Generic;
using FT.Shooting;
using FT.TD;
using UnityEngine;

namespace FT.Ability.States
{
    public class ExplosionState : AbilityState
    {
        [SerializeField] private Transform _explosionEffect;
        private float _delayBeforeDestroy;
        
        private void Awake()
        {
            foreach (ParticleSystem ps in _explosionEffect.GetComponentsInChildren<ParticleSystem>())
            {
                if (ps.main.startLifetime.constantMax < _delayBeforeDestroy)
                    continue;

                _delayBeforeDestroy = ps.main.startLifetime.constantMax;
            }
        }

        public override void Execute(List<Data.Ability> abilities)
        {
            StatsController.ApplyDamage(AbilityInfo.Damage);
            Collider[] hits = Physics.OverlapSphere(transform.position, 2.0f);
            foreach (Collider col in hits)
            {
                if (col.transform == StatsController.transform || col.transform.TryGetComponent(out IHit hit) == false)
                    continue;
                
                ((CharacterStatsController)hit).ApplyDamage(AbilityInfo.Damage);
                hit.RegisterAbilityStates(abilities);
            }
            
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject, _delayBeforeDestroy);
        }
    }
}