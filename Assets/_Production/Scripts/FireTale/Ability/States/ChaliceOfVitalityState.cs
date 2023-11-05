using System.Collections;
using UnityEngine;

namespace FT.Ability.States
{
    public class ChaliceOfVitalityState : AbilityState
    {
        public override void Execute() => 
            StartCoroutine(nameof(ExecuteAtEndOfFrame));

        private IEnumerator ExecuteAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            if (StatsController.health > 0)
            {
                AbilityEnd();
                yield break;
            }

            PlayerStatController.health += 10;
            AbilityEnd();
        }
    }
}