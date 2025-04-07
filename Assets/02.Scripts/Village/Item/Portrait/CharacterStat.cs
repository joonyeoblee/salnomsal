using System;

namespace Portrait
{
    [Serializable]
    public class CharacterStat
    {
        public float MaxHealth;
        public float MaxMana;
        public float AttackPower;

        public CharacterStat(float MaxHealth, float MaxMana, float AttackPower)
        {
            this.MaxHealth = MaxHealth;
            this.MaxMana = MaxMana;
            this.AttackPower = AttackPower;
        }
    }
}
