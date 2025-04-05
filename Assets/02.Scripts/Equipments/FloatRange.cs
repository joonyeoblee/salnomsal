using System;

namespace Equipment
{
    [Serializable]
    public class FloatRange
    {
        public float Min;
        public float Max;

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}