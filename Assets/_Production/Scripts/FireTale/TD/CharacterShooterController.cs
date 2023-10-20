using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FT.Ability;
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
        [SerializeField] private List<Data.Ability> _abilities;

        private float _nextShootTime;
        
        private void Awake() => GetComponent<Character>()?.State.IsShooting.AddObserver(ToggleShooting);

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
        
        private void OnHit(IHit hit)
        {
            List<IAbilityState> abilityStates = _abilities.Select(ability => Instantiate(ability.Prefab, GameObject.FindWithTag("Dynamic").transform).GetComponent<IAbilityState>()).ToList();
            HashSet<IHit> hitObjects = new();

            foreach (IAbilityState abilityState in abilityStates)
                abilityState.SingleTarget(hit);
            
            foreach (IAbilityState abilityState in abilityStates)
                hitObjects.UnionWith(abilityState.GatherData(hit));

            foreach (IAbilityState abilityState in abilityStates)
                abilityState.ExecuteCall(hitObjects);
        }
    }
}
