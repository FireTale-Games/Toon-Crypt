using System.Collections;
using FT.Shooting;
using UnityEngine;

namespace FT.Ability.States
{
    public class PoisonState : AbilityState
    {
        public override void SingleTarget(IHit hit) =>
            StartCoroutine(HitRepeatable(hit));

        private IEnumerator HitRepeatable(IHit hit)
        {
            byte index = 0;
            while (index < 3)
            {
                yield return new WaitForSeconds(1);
                hit.Damage(2);

                index++;
            }
        }
    }
}