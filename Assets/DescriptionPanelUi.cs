using FT.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FT.UI
{
    public class DescriptionPanelUi : MonoBehaviour
    {
        [SerializeField] private Image _abilityImage;
        [SerializeField] private TextMeshProUGUI _abilityName;
        [SerializeField] private TextMeshProUGUI _abilityDescription;
        [SerializeField] private CanvasGroup _descriptionPanelGroup;

        public void DisplayItem(Item item)
        {
            _abilityImage.sprite = item.Sprite;
            _abilityName.text = item.DisplayName;
            _abilityDescription.text = item.DisplayName;
            ToggleVisibility(true);
        }

        public void ToggleVisibility(bool value) =>
            _descriptionPanelGroup.alpha = value ? 1 : 0;
    }
}