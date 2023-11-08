using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FT.Data;
using FT.Inventory;
using FT.TD;
using FT.Tools.Observers;
using FT.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FT.Shooting
{
    public class CharacterShooterController : MonoBehaviour
    {
        [SerializeField] private float _shootDelay;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _spawnPoint;
        private readonly Dictionary<int, Data.Ability> _abilities = new();

        private float _nextShootTime;
        
        private void Awake()
        {
#if UNITY_EDITOR
            DebugAbilities();
#endif
            
            CharacterState state = GetComponent<Character>()?.State;
            state?.IsShooting.AddObserver(ToggleShooting);

            IInventory _inventory = GetComponent<IInventory>();
            _inventory.OnAbilityUpdate.AddObserver(OnAbilityChange);
            _inventory.OnWeaponUpdate.AddObserver(OnWeaponChange);
        }

        private void OnWeaponChange(InventoryItem weaponItem)
        {
            _abilities.Clear();
            if (!weaponItem.IsValid)
                return;

            for (int i = 0; i < weaponItem._abilities.Length; i++)
            {
                Data.Ability ability = weaponItem._abilities[i] == 0 ? null : ItemDatabase.Get<Data.Ability>(weaponItem._abilities[i]);
                if (ability != null)
                    _abilities.Add(i, ability);
            }
        }

        private void OnAbilityChange(int id, int index)
        {
            if (id == 0) _abilities.Remove(index);
            else _abilities[index] = ItemDatabase.Get<Data.Ability>(id);
        }

        private void ToggleShooting(bool value)
        {
            if (value) StartCoroutine(nameof(StartShooting));
            else StopAllCoroutines();
        }

        private IEnumerator StartShooting()
        {
            if (_nextShootTime <= Time.time)
            {
                ShootProjectile().AddObserver(OnHit);
                
                _nextShootTime = Time.time + _shootDelay;
                yield return new WaitForSeconds(_shootDelay);
            }
            
            while (true)
            {
                if (_nextShootTime <= Time.time)
                {
                    ShootProjectile().AddObserver(OnHit);
                    
                    _nextShootTime = Time.time + _shootDelay;
                }

                yield return null;
            }
        }
        
        private IObservableAction<Action<IHit>> ShootProjectile()
        {
            Projectile projectile = Instantiate(_projectile, _spawnPoint);
            projectile.transform.SetParent(GameObject.FindWithTag("Dynamic").transform);

            return projectile.OnHit;
        }
        
        private void OnHit(IHit hit) => hit.RegisterAbilityStates(ApplyAbilities(), GetComponent<CharacterStatsController>());

        private List<Data.Ability> ApplyAbilities()
        {
            List<Data.Ability> abilities = _abilities.Values.ToList();
            bool isCritical = Random.Range(0, 100) < 40;
            if (!isCritical)
                abilities = abilities.Where(ability => ability.RestrictionType != AbilityRestrictionType.Critical).ToList();
            
            return abilities;
        }
        
        #region GAME_DEBUG
#if UNITY_EDITOR
        [SerializeField] private List<Data.Ability> _debugAbilities = new();

        private void DebugAbilities()
        {
            for (int i = 0; i < _debugAbilities.Count; i++)
                _abilities[i] = _debugAbilities[i];
        }
    }
#endif  
    #endregion
}
