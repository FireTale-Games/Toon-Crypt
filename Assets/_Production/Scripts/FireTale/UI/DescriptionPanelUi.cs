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

        private CanvasGroup _canvasGroup;

        private void Awake() => 
            _canvasGroup = GetComponent<CanvasGroup>();

        public void EnableDisplay(Item ability)
        {
            _abilityImage.sprite = ability.Sprite;
            _abilityName.text = ability.DisplayName;
            _abilityDescription.text = ability.DisplayName;
            _canvasGroup.alpha = 1.0f;
        }
        
        public void DisableDisplay() => 
            _canvasGroup.alpha = 0.0f;
    }
}