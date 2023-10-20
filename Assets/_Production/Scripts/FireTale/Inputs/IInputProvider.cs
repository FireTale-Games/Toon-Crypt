using System;
using FT.Tools.Observers;

namespace FT.Inputs
{
    public interface IInputProvider
    {
        public IObservableAction<Action<InputData>> OnInput { get; }
}
}
