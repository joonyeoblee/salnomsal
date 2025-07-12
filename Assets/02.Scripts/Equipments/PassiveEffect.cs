using System;
using UnityEngine;
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

    [Serializable]
    public class FloatRange
    {
        public float Min;
        public float Max;

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

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

        // float 네 개로 받는 오버로드 추가
        public PassiveEffect(PassiveType passiveType, float common, float rare, float epic, float legendary)
        {
            PassiveType = passiveType;
            CommonRange = new FloatRange(common, common);
            RareRange = new FloatRange(rare, rare);
            EpicRange = new FloatRange(epic, epic);
            LegendaryRange = new FloatRange(legendary, legendary);
        }


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