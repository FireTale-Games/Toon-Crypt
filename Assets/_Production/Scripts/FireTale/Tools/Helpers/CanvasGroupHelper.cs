using System;
using DG.Tweening;
using UnityEngine;

namespace FT.Tools.Helpers
{
    public static class CanvasGroupHelper
    {
        public static void ShowCanvasGroup(this CanvasGroup self, float fadeDuration, float delay = 0,
            Ease ease = Ease.Linear, Action onStart = null, Action onComplete = null)
        {
            self.DOFade(1, fadeDuration)
                .SetDelay(delay)
                .SetEase(ease)
                .OnStart(() => onStart?.Invoke())
                .OnComplete(() =>
                {
                    self.interactable = true;
                    self.blocksRaycasts = true;
                    onComplete?.Invoke();
                })
                .Play();
        }

        public static void HideCanvasGroup(this CanvasGroup self, float fadeDuration, float delay = 0,
            Ease ease = Ease.Linear, Action onStart = null, Action onComplete = null)
        {
            self.interactable = false;
            self.blocksRaycasts = false;
            self.DOFade(0, fadeDuration)
                .SetDelay(delay)
                .SetEase(ease)
                .OnStart(() => onStart?.Invoke())
                .OnComplete(() => onComplete?.Invoke())
                .Play();
        }
    }
}