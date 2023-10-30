using System;
using FT.Tools.Extensions;
using FT.Tools.Helpers;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.UI
{
    public class ScreenBase : MonoBehaviour, IScreen
    {
        public IObservableAction<Action<IScreen>> OnRequestToOpen => _onRequestToOpen;
        private readonly ObservableAction<Action<IScreen>> _onRequestToOpen = new();
        
        public IObservableAction<Action> OnRequestToClose => _onRequestToClose;
        private readonly ObservableAction<Action> _onRequestToClose = new();
        
        protected CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;
        
        protected Canvas Canvas => _canvas ??= GetComponent<Canvas>();
        private Canvas _canvas;

        private int SortOrderOnOpen => 4;
        
        protected virtual void Start()
        {
            CanvasGroup.Null()?.HideCanvasGroup(0);
            if(Canvas != null)
                Canvas.sortingOrder = 1;
        }

        public virtual void Show()
        {
            CanvasGroup.Null()?.ShowCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = SortOrderOnOpen;
        }

        public virtual void Hide()
        {
            CanvasGroup.Null()?.HideCanvasGroup(0.1f);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
        }

    }
}