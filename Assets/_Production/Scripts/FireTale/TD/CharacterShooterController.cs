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
            else _abilities[index] = ItemDatabase.Get<Data.Ability>(index);
        }

        private void ToggleShooting(bool value)
        {
            if (value) StartCoroutine(nameof(StartShooting));
            else StopAllCoroutines();
        }

        private IEnumerator StartShooting()
        {
            yield break;
            
            //if (_nextShootTime <= Time.time)
            //{
            //    ShootProjectile().AddObserver(OnHit);
            //    
            //    _nextShootTime = Time.time + _shootDelay;
            //    yield return new WaitForSeconds(_shootDelay);
            //}
            //
            //while (true)
            //{
            //    if (_nextShootTime <= Time.time)
            //    {
            //        ShootProjectile().AddObserver(OnHit);
            //        
            //        _nextShootTime = Time.time + _shootDelay;
            //    }

            //    yield return null;
            //}
        }
        
        private IObservableAction<Action<IHit>> ShootProjectile()
        {
            Projectile projectile = Instantiate(_projectile, _spawnPoint);
            projectile.transform.SetParent(GameObject.FindWithTag("Dynamic").transform);

            return projectile.OnHit;
        }
        
        private void OnHit(IHit hit) => hit.RegisterAbilityStates(_abilities.Values.ToList());
    }
}
