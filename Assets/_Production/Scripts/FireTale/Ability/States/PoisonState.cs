using System.Collections;
using UnityEngine;

namespace FT.Ability.States
{
    public class PoisonState : AbilityState
    {
        private const float _duration = 10.0f;
        private float _currentDuration = 0.0f;
        
        public override void ResetDuration() => 
            _currentDuration = 0.0f;

        public override void Execute() => 
            StartCoroutine(nameof(PoisonAttack));

        private IEnumerator PoisonAttack()
        {
            while (_currentDuration < _duration)
            {
                yield return new WaitForSeconds(1);
                _currentDuration += 1.0f;

                //StatsController.ApplyDamage(AbilityInfo.Damage);
            }
            
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject);
        }
    }
}