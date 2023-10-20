using UnityEngine;

namespace FT.Shooting
{
    public interface IHit
    {
        public void Damage(float damage);
        public Vector3 Position { get; }
    }
}