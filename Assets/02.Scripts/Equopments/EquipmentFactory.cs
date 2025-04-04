using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public static class EquipmentFactory
    {
        public static EquipmentInstance Create(EquipmentSO template, Rarity overrideRarity = Rarity.Common)
        {
            var baseStats = new List<StatModifier>();
            foreach (var statRange in template.BaseStatRanges)
            {
                float value = Random.Range(statRange.MinValue, statRange.MaxValue);
                baseStats.Add(new StatModifier
                {
                    StatType = statRange.StatType,
                    Value = value
                });
            }

            var appliedPassives = new List<AppliedPassiveEffect>();
            foreach (var passive in template.PassiveEffects)
            {
                float value = passive.GetRandomValueByRarity(overrideRarity);
                appliedPassives.Add(new AppliedPassiveEffect
                {
                    PassiveType = passive.PassiveType,
                    Value = value
                });
            }

            return new EquipmentInstance(template, baseStats, appliedPassives);
        }
    }
}