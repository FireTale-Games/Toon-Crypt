using System;

namespace FT.Tools.Observers
{
    public interface IObservableProperty<out T> : IObservableAction<Action<T>>
    {
        public T Value { get; }
    }
}