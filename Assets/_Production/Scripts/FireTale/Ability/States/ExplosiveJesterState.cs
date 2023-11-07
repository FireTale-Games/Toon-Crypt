using System;
using System.Collections.Generic;
using FT.Shooting;
using Newtonsoft.Json;
using UnityEngine;

namespace FT.Ability.States
{
    public class ExplosiveJesterState : AbilityState
    {
        private class ExplosiveJesterStateParameters
        {
            public float Radius { get; set; }
            public float Damage { get; set; }
        }

        private ExplosiveJesterStateParameters _parameters;

        [SerializeField] private Transform _particle;

        protected override void AssignParameters() =>
            _parameters = JsonConvert.DeserializeObject<ExplosiveJesterStateParameters>(AbilityInfo.AbilityParameters);

        public override void Execute(List<Data.Ability> abilities)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _parameters.Radius);
            foreach (Collider col in hits)
            {
                if (col.transform == StatsController.transform || col.transform.TryGetComponent(out IHit hit) == false)
                    continue;
                
                hit.ApplyFlatDamage(_parameters.Damage, _particle);
                hit.RegisterAbilityStates(abilities, PlayerStatController);
            }
            
            StatsController.ApplyFlatDamage(_parameters.Damage, _particle);
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject);
        }
    }
}