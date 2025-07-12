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
        static Rarity GetRandomRarity()
        {
            float rand = Random.value;

            if (rand < 0.5f) return Rarity.Common; // 50%
            if (rand < 0.8f) return Rarity.Rare; // 30%
            if (rand < 0.95f) return Rarity.Epic; // 15%
            return Rarity.Legendary; // 5%
        }
        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        public static EquipmentInstance Create(EquipmentSO template)
        {
            Rarity rarity = GetRandomRarity();

            List<StatModifier> baseStats = new();
            foreach (StatRange statRange in template.BaseStatRanges)
            {
                float value = Random.Range(statRange.MinValue, statRange.MaxValue);
                baseStats.Add(new StatModifier(statRange.StatType, value));
            }

            List<AppliedPassiveEffect> appliedPassives = new();

            List<PassiveEffect> pool = new(template.PassiveEffects);
            Shuffle(pool);
            int passiveCount = GetPassiveCountByRarity(rarity);
            List<PassiveEffect> selected = pool.Take(passiveCount).ToList();

            foreach (PassiveEffect passive in selected)
            {
                float value = passive.GetRandomValueByRarity(rarity);
                appliedPassives.Add(new AppliedPassiveEffect(passive.PassiveType, value));
            }

            // return new EquipmentInstance(template, rarity, baseStats, appliedPassives);
            return null;
        }

        
    }
}