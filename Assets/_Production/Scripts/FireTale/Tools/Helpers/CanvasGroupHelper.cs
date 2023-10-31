using UnityEngine;

namespace FT.Tools.Helpers
{
    public static class CanvasGroupHelper
    {
        public static void ShowCanvasGroup(this CanvasGroup self)
        {
            self.alpha = 1;
            self.interactable = true;
            self.blocksRaycasts = true;
        }

        public static void HideCanvasGroup(this CanvasGroup self)
        {
            self.alpha = 0;
            self.interactable = false;
            self.blocksRaycasts = false;
        }
    }
}