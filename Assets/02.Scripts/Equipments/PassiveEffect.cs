using System;

namespace Equipment
{
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