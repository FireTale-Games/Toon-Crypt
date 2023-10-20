using FT.Shooting;
using UnityEngine;

namespace FT.TD
{
    public class CharacterStatsController : MonoBehaviour, IHit
    {
        public Vector3 Position => transform.position;
        
        public float health = 100;
        
        public void Damage(float damage)
        {
            health -= damage;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}
