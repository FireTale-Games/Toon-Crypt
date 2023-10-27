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

        public void DisplayItem(Data.Ability ability)
        {
            _abilityImage.sprite = ability.Sprite;
            _abilityName.text = ability.DisplayName;
            _abilityDescription.text = ability.DisplayName;
        }
    }
}