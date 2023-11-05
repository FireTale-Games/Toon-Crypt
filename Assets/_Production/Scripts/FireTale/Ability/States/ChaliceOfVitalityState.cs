using System.Collections;
using UnityEngine;

namespace FT.Ability.States
{
    public class ChaliceOfVitalityState : AbilityState
    {
        [SerializeField] private Transform _healParticle;
        
        public override void Execute() => 
            StartCoroutine(nameof(ExecuteAtEndOfFrame));

        private IEnumerator ExecuteAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            if (StatsController.Health > 0)
            {
                AbilityEnd();
                yield break;
            }

            PlayerStatController.ApplyHeal(10, _healParticle);
            AbilityEnd();
        }
    }
}