using System;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.Ability
{
    public abstract class AbilityState : MonoBehaviour, IAbilityState
    {
        public IObservableAction<Action<IAbilityState>> OnDispose => _onDispose;
        protected readonly ObservableAction<Action<IAbilityState>> _onDispose = new();
        
        public virtual void ResetDuration() { }
        public virtual void Execute() { }
    }
}