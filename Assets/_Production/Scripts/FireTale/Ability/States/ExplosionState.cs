using System.Collections.Generic;
using FT.Shooting;
using FT.TD;
using UnityEngine;

namespace FT.Ability.States
{
    public class ExplosionState : AbilityState
    {
        public override void Execute(List<Data.Ability> abilities)
        {
            //StatsController.ApplyDamage(AbilityInfo.Damage);
            Collider[] hits = Physics.OverlapSphere(transform.position, 2.0f);
            foreach (Collider col in hits)
            {
                if (col.transform == StatsController.transform || col.transform.TryGetComponent(out IHit hit) == false)
                    continue;
                
                //((CharacterStatsController)hit).ApplyDamage(AbilityInfo.Damage);
                hit.RegisterAbilityStates(abilities);
            }
            
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject);
        }
    }
}