using System;

namespace Equipment
{
    [Serializable]
    public class AppliedPassiveEffect
    {
        public PassiveType PassiveType;
        public float Value;
        
        public AppliedPassiveEffect(PassiveType passiveType, float value)
        {
            PassiveType = passiveType;
            Value = value;
        }
    }
}