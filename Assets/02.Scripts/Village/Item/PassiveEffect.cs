using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class PassiveEffect
    {
        public PassiveType passiveType;
        [Header("등급별 수치")] public int CommonValue;
        public int RareValue;
        public int EpicValue;
        public int LegendaryValue;

        public int GetValueByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => CommonValue,
                Rarity.Rare => RareValue,
                Rarity.Epic => EpicValue,
                Rarity.Legendary => LegendaryValue,
                _ => 0
            };
        }
    }
}