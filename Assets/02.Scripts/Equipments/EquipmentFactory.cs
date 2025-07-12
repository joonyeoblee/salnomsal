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

        private static Rarity GetRandomRarity()
        {
            float rand = Random.value;
            if (rand < 0.5f) return Rarity.Common;      // 50%
            if (rand < 0.8f) return Rarity.Rare;         // 30%
            if (rand < 0.95f) return Rarity.Epic;        // 15%
            return Rarity.Legendary;                     // 5%
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        /// <summary>
        /// 장비 인스턴스만 생성
        /// </summary>
        public static EquipmentInstance Create(
            EquipmentType type,
            string iconAddress,
            string borderAddress,
            List<StatRange> statRanges,
            List<PassiveEffect> passivePool)
        {
            Rarity rarity = GetRandomRarity();

            List<StatModifier> baseStats = new();
            foreach (var statRange in statRanges)
            {
                float value = Random.Range(statRange.MinValue, statRange.MaxValue);
                baseStats.Add(new StatModifier(statRange.StatType, value));
            }

            List<AppliedPassiveEffect> appliedPassives = new();
            if (passivePool != null && passivePool.Count > 0)
            {
                List<PassiveEffect> poolCopy = new(passivePool);
                Shuffle(poolCopy);
                int count = GetPassiveCountByRarity(rarity);

                foreach (var passive in poolCopy.Take(count))
                {
                    float value = passive.GetRandomValueByRarity(rarity);
                    appliedPassives.Add(new AppliedPassiveEffect(passive.PassiveType, value));
                }
            }

            return new EquipmentInstance(
                type,
                iconAddress,
                borderAddress,
                rarity,
                baseStats,
                appliedPassives
            );
        }

        /// <summary>
        /// 장비 생성 후 인벤토리에 바로 추가
        /// </summary>
        public static void CreateAndAddToInventory(
            EquipmentType type,
            string iconAddress,
            string borderAddress,
            List<StatRange> statRanges,
            List<PassiveEffect> passivePool)
        {
            EquipmentInstance item = Create(type, iconAddress, borderAddress, statRanges, passivePool);
            EquipmentManager.Instance.AddItem(item);
        }
    }
}
