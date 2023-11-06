using System;
using System.Collections.Generic;
using FT.Data;
using FT.TD;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.Ability
{
    public abstract class AbilityState : MonoBehaviour, IAbilityState
    {
        private int Id;
        
        protected CharacterStatsController StatsController => _statsController ??= GetComponentInParent<CharacterStatsController>();
        private CharacterStatsController _statsController;

        protected CharacterStatsController PlayerStatController;
        
        public IObservableAction<Action<IAbilityState>> OnDispose => _onDispose;
        protected readonly ObservableAction<Action<IAbilityState>> _onDispose = new();

        public Data.Ability AbilityInfo => _ability ??= ItemDatabase.Get<Data.Ability>(Id);
        private Data.Ability _ability;
        
        public void Initialize(int id, CharacterStatsController playerStats = null)
        {
            Id = id;
            PlayerStatController = playerStats;
            AssignParameters();
        }

        public virtual void ResetDuration() { }
        public virtual void Execute() { }
        public virtual void Execute(List<Data.Ability> abilities) { }
        protected virtual void AssignParameters() { }
        protected virtual void AbilityEnd()
        {
            _onDispose.Action?.Invoke(this);
            Destroy(gameObject);
        }
    }
}