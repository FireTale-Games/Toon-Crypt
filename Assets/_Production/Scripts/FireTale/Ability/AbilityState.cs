using System;
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
        
        public IObservableAction<Action<IAbilityState>> OnDispose => _onDispose;
        protected readonly ObservableAction<Action<IAbilityState>> _onDispose = new();

        public Data.Ability AbilityInfo => _ability ??= ItemDatabase.Get<Data.Ability>(Id);
        private Data.Ability _ability;
        
        public void Initialize(int id) => Id = id;
        public virtual void ResetDuration() { }
        public virtual void Execute() { }
    }
}