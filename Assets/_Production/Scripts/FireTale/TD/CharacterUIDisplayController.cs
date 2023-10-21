using FT.Ability;
using UnityEngine;
using UnityEngine.UI;

namespace FT.TD
{
    public class CharacterUIDisplayController : MonoBehaviour
    {
        [SerializeField] private Image _healthBarPanel;
        [SerializeField] private Transform _statEffectsGroup;
        [SerializeField] private Image _abilityUIEffect;
        
        private void Awake()
        {
            CharacterStatsController statsController = GetComponentInParent<CharacterStatsController>();
            statsController.OnDamageReceived.AddObserver(UpdateHealthBar);
            statsController.OnAbilityRegister.AddObserver(RegisterAbility);
            statsController.OnAbilityUnregister.AddObserver(UnregisterAbility);
        }

        private void RegisterAbility(IAbilityState abilityState)
        {
            Image abilitySprite = Instantiate(_abilityUIEffect, _statEffectsGroup);
            abilitySprite.sprite = abilityState.AbilityInfo.Sprite;
            abilitySprite.name = abilityState.AbilityInfo.Id.ToString();
        }

        private void UnregisterAbility(IAbilityState abilityState)
        {
            int Id = abilityState.AbilityInfo.Id;
            int effectNumber = _statEffectsGroup.childCount;
            for (int i = 0; i < effectNumber; i++)
            {
                if (_statEffectsGroup.GetChild(i).name != Id.ToString())
                    continue;
                
                Destroy(_statEffectsGroup.GetChild(i).gameObject);
                break;
            }
        }

        private void UpdateHealthBar(float health) => 
            _healthBarPanel.fillAmount = health / 100.0f;
    }
}