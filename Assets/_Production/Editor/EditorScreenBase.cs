using UnityEngine.UIElements;

namespace Editor
{
    public abstract class EditorScreenBase
    {
        public virtual void SaveData() { }
        protected abstract void BindData(VisualElement parentElement);
    }
}