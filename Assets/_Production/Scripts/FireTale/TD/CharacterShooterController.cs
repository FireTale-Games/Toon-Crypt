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
        [SerializeField] private List<Data.Ability> _abilities;

        private float _nextShootTime;
        
        private void Awake()
        {
            CharacterState state = GetComponent<Character>()?.State;
            state?.IsShooting.AddObserver(ToggleShooting);
            state?.IsInventory.AddObserver(v => { if (v) StopAllCoroutines(); });
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
//
            //    yield return null;
            //}
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
