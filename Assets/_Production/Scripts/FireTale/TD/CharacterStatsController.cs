using System;
using System.Collections.Generic;
using System.Linq;
using FT.Ability;
using FT.Data;
using FT.Shooting;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.TD
{
    public class CharacterStatsController : MonoBehaviour, IHit
    {
        public float health = 100.0f;
        
        public IObservableAction<Action<IAbilityState>> OnAbilityRegister => _onAbilityRegister;
        private readonly ObservableAction<Action<IAbilityState>> _onAbilityRegister = new();
        
        public IObservableAction<Action<float>> OnDamageReceived => _onDamageReceived;
        private readonly ObservableAction<Action<float>> _onDamageReceived = new();
        
        public IObservableAction<Action<IAbilityState>> OnAbilityUnregister => _onAbilityUnregister;
        private readonly ObservableAction<Action<IAbilityState>> _onAbilityUnregister = new();
        
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
                abilityState.Initialize(ability.Id);
                abilityState.OnDispose.AddObserver(RemoveAbilityState);
                
                _abilityStates.Add(abilityState);
                _onAbilityRegister.Action?.Invoke(abilityState);

                if (ability.HitType == AbilityHitType.SINGLE_TARGET)
                    abilityState.Execute();
                else
                    abilityState.Execute(abilities.Where(value => value.HitType == AbilityHitType.SINGLE_TARGET).ToList());
            }
        }

        private void RemoveAbilityState(IAbilityState abilityState)
        {
            if (!_abilityStates.Contains(abilityState))
                return;
            
            _onAbilityUnregister.Action?.Invoke(abilityState);
            _abilityStates.Remove(abilityState);
        }

        public void ApplyDamage(float damage)
        {
            health -= damage;
            _onDamageReceived.Action?.Invoke(health);
        }
    }
}
