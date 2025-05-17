using System;
using Random = UnityEngine.Random;

namespace Equipment
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }


    public class FloatRange
    {
        public float Min;
        public float Max;

        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }

    [Serializable]
    public class PassiveEffect
    {
        public PassiveType PassiveType;

        public FloatRange CommonRange;
        public FloatRange RareRange;
        public FloatRange EpicRange;
        public FloatRange LegendaryRange;

        public float GetRandomValueByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => CommonRange.GetRandom(),
                Rarity.Rare => RareRange.GetRandom(),
                Rarity.Epic => EpicRange.GetRandom(),
                Rarity.Legendary => LegendaryRange.GetRandom(),
                _ => 0f
            };
        }
    }
}