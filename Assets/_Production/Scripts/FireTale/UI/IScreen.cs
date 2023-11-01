using System;
using FT.Tools.Observers;

namespace FT.UI
{
    public interface IScreen
    {
        public IObservableAction<Action> OnRequestToClose { get; }
        public IObservableAction<Action<IScreen>> OnRequestToOpen { get; }
        public void Show();
        public void Hide();
    }
}
