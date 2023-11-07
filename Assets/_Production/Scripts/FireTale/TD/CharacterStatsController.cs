using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FT.Ability;
using FT.Data;
using FT.Shooting;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.TD
{
    public class CharacterStatsController : MonoBehaviour, IHit
    {
        [field: SerializeField] public float Health { get; private set; } = 100.0f;
        
        public IObservableAction<Action<IAbilityState>> OnAbilityRegister => _onAbilityRegister;
        private readonly ObservableAction<Action<IAbilityState>> _onAbilityRegister = new();
        
        public IObservableAction<Action<float>> OnDamageReceived => _onDamageReceived;
        private readonly ObservableAction<Action<float>> _onDamageReceived = new();
        
        public IObservableAction<Action<IAbilityState>> OnAbilityUnregister => _onAbilityUnregister;
        private readonly ObservableAction<Action<IAbilityState>> _onAbilityUnregister = new();
        
        private readonly List<IAbilityState> _abilityStates = new();

        public void RegisterAbilityStates(List<Data.Ability> abilities, CharacterStatsController playerController)
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
                abilityState.Initialize(ability.Id, playerController);
                abilityState.OnDispose.AddObserver(RemoveAbilityState);
                
                _abilityStates.Add(abilityState);
                _onAbilityRegister.Action?.Invoke(abilityState);

                if (ability.SpreadType == AbilitySpreadType.Single_Target)
                    abilityState.Execute();
                else
                    abilityState.Execute(abilities.Where(value => value.SpreadType == AbilitySpreadType.Single_Target).ToList());
            }
        }

        private void RemoveAbilityState(IAbilityState abilityState)
        {
            if (!_abilityStates.Contains(abilityState))
                return;
            
            _onAbilityUnregister.Action?.Invoke(abilityState);
            _abilityStates.Remove(abilityState);
        }

        public void ApplyHeal(float healAmount, Transform healParticle = null)
        {
            float amount = Health + healAmount;
            Health = amount > 100 ? 100 : amount;

            if (healParticle == null)
                return;

            Transform particle = Instantiate(healParticle, transform);
            float duration = GetComponentsInChildren<ParticleSystem>().Select(ps => ps.main.duration).Prepend(0).Max();
            DOVirtual.DelayedCall(duration, () => Destroy(particle.gameObject));
        }
        
        public void ApplyFlatDamage(float damage, Transform damageParticle = null)
        {
            Health -= damage;
            _onDamageReceived.Action?.Invoke(Health);
            
            if (damageParticle == null)
                return;

            Transform particle = Instantiate(damageParticle, transform);
            float duration = GetComponentsInChildren<ParticleSystem>().Select(ps => ps.main.duration).Prepend(0).Max();
            DOVirtual.DelayedCall(duration, () => Destroy(particle.gameObject));
        }
    }
}
