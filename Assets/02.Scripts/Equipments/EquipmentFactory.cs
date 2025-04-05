using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Equipment
{
    public static class EquipmentFactory
    {
        private static int GetPassiveCountByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 0,
                Rarity.Rare => 1,
                Rarity.Epic => 2,
                Rarity.Legendary => 3,
                _ => 0
            };
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
        
        public static EquipmentInstance Create(EquipmentSO template, Rarity overrideRarity = Rarity.Common)
        {
            List<StatModifier> baseStats = new();
            foreach (StatRange statRange in template.BaseStatRanges)
            {
                float value = Random.Range(statRange.MinValue, statRange.MaxValue);
                baseStats.Add(new StatModifier
                {
                    StatType = statRange.StatType,
                    Value = value
                });
            }

            List<AppliedPassiveEffect> appliedPassives = new();

            List<PassiveEffect> pool = new(template.PassiveEffects);
            Shuffle(pool);
            int passiveCount = GetPassiveCountByRarity(overrideRarity);
            List<PassiveEffect> selected = pool.Take(passiveCount).ToList();

            foreach (PassiveEffect passive in selected)
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