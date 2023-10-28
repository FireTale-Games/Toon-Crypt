using System.Collections;
using UnityEngine;

namespace FT.Ability.States
{
    public class BurnState : AbilityState
    {
        private const float _duration = 14.0f;
        private float _currentDuration;
        
        public override void ResetDuration() => 
            _currentDuration = 0.0f;

        public override void Execute() => 
            StartCoroutine(nameof(BurnAttack));

        private IEnumerator BurnAttack()
        {
            while (_currentDuration < _duration)
            {
                yield return new WaitForSeconds(2.0f);
                _currentDuration += 2.0f;

                //StatsController.ApplyDamage(AbilityInfo.Damage);
            }
            
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject);
        }
    }
}