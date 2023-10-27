using System;
using System.Collections.Generic;
using FT.Tools.Observers;

namespace FT.Ability
{
    public interface IAbilityState
    {
        public Data.Ability AbilityInfo { get; }
        public void Initialize(int id);
        public void ResetDuration();
        public void Execute();
        public void Execute(HashSet<Data.Ability> abilities);
        public IObservableAction<Action<IAbilityState>> OnDispose { get; }
    }
}