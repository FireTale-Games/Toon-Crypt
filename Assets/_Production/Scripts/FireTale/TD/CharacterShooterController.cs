using System;
using System.Collections;
using System.Collections.Generic;
using FT.Data;
using FT.TD;
using FT.Tools.Observers;
using UnityEngine;

namespace FT.Shooting
{
    public class CharacterShooterController : MonoBehaviour
    {
        [SerializeField] private float _shootDelay;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private Transform _spawnPoint;
        
        private readonly HashSet<Data.Ability> _abilities = new();
        private float _nextShootTime;
        
        private void Awake()
        {
            Character character = GetComponent<Character>();
            character.State.IsShooting.AddObserver(ToggleShooting);
            character.State.AddSpell.AddObserver(AddSpell);
        }

        private void AddSpell(AbilityStruct spell)
        {
            Data.Ability ability = ItemDatabase.Get<Data.Ability>(spell._id);
            if (ability == null)
                return;
            
            if (spell._isAdd)
                _abilities.Add(ability);
            else
                _abilities.Remove(ability);
            
            Debug.Log(_abilities.Count);
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
        
        private void OnHit(IHit hit) => hit.RegisterAbilityStates(_abilities);
    }
}
