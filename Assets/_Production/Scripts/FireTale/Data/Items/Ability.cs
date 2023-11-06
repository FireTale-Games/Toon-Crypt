using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public enum AbilitySpreadType : byte { SINGLE_TARGET, MULTIPLE_TARGETS }

    public class Ability : Item
    {
        [field: SerializeField] public AbilitySpreadType SpreadType { get; private set; }
        [field: SerializeField] public string AbilityParameters { get; private set; }
        
#if UNITY_EDITOR
        public override void Setup(DataRow data)
        {
            base.Setup(data);

            SpreadType = data.Parse<AbilitySpreadType>(nameof(SpreadType));
            AbilityParameters = data.Parse<string>(nameof(AbilityParameters));
        }
#endif
    }
}