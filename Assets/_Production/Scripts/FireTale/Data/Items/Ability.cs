using UnityEngine;

#if UNITY_EDITOR
using System.Data;
using FT.Tools.Extensions;
#endif

namespace FT.Data
{
    public enum AbilitySpreadType : byte { Single_Target, Multiple_Targets }
    public enum AbilityRestrictionType : byte { None, Critical }

    public class Ability : Item
    {
        [field: SerializeField] public AbilitySpreadType SpreadType { get; private set; }
        [field: SerializeField] public AbilityRestrictionType RestrictionType { get; private set; }
        [field: SerializeField] public string AbilityParameters { get; private set; }
        
#if UNITY_EDITOR
        public override void Setup(DataRow data)
        {
            base.Setup(data);

            SpreadType = data.Parse<AbilitySpreadType>(nameof(SpreadType));
            RestrictionType = data.Parse<AbilityRestrictionType>(nameof(RestrictionType));
            AbilityParameters = data.Parse<string>(nameof(AbilityParameters));
        }
#endif
    }
}