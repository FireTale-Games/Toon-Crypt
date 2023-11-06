using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace FT.Ability.States
{
    public class ChaliceOfVitalityState : AbilityState
    {
        private class ChaliceOfVitalityParameters
        {
            public float Health { get; set; }
        }

        private ChaliceOfVitalityParameters _parameters;
        
        [SerializeField] private Transform _healParticle;

        protected override void AssignParameters() => 
            _parameters = JsonConvert.DeserializeObject<ChaliceOfVitalityParameters>(AbilityInfo.AbilityParameters);

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