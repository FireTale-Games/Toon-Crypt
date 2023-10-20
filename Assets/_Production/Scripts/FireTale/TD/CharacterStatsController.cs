using System.Collections.Generic;
using System.Linq;
using FT.Ability;
using FT.Shooting;
using UnityEngine;

namespace FT.TD
{
    public class CharacterStatsController : MonoBehaviour, IHit
    {
        public float health = 100.0f;
        private readonly List<IAbilityState> _abilityStates = new();

        public void RegisterAbilityStates(List<Data.Ability> abilities)
        {
            foreach (Data.Ability ability in abilities)
            {
                IAbilityState abilityState = _abilityStates.FirstOrDefault(a => a.GetType() == ability.Prefab.GetComponent<IAbilityState>().GetType());
                if (abilityState != null)
                {
                    abilityState.ResetDuration();
                    continue;
                }

                abilityState = Instantiate(ability.Prefab, transform).GetComponent<IAbilityState>();
                abilityState.OnDispose.AddObserver(RemoveAbilityState);
                abilityState.Execute();
                
                _abilityStates.Add(abilityState);
            }
        }

        private void RemoveAbilityState(IAbilityState abilityState)
        {
            if (_abilityStates.Contains(abilityState))
                _abilityStates.Remove(abilityState);
        }
    }
}
