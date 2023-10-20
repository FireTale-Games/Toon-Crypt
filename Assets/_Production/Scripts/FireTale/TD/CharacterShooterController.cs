using System.Collections;
using UnityEngine;

namespace FT.TD
{
    public class CharacterShooterController : MonoBehaviour
    {
        [SerializeField] private float _shootDelay;

        private float _nextShootTime;
        
        private void Awake()
        {
            GetComponent<Character>()?.State.IsShooting.AddObserver(ToggleShooting);
        }

        private void ToggleShooting(bool value)
        {
            if (value)
                StartCoroutine(nameof(StartShooting));
            else
            {
                Debug.Log("stop");
                StopAllCoroutines();
            }
        }

        private IEnumerator StartShooting()
        {
            if (_nextShootTime <= Time.time)
            {
                // Shoot immediately
                _nextShootTime = Time.time + _shootDelay;
                yield return new WaitForSeconds(_shootDelay);
            }
            
            while (true)
            {
                if (_nextShootTime <= Time.time)
                {
                    _nextShootTime = Time.time + _shootDelay;
                }

                yield return null;
            }
        }
    }
}
