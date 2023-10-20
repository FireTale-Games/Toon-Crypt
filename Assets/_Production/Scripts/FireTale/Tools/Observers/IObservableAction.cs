using System;

namespace FT.Tools.Observers
{
    public interface IObservableAction<in T> where T : Delegate
    {
        public void AddObserver(T observer);
        public void RemoveObserver(T observer);
    }
}