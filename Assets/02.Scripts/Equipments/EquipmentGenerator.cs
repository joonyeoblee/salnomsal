using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Equipment
{
    public enum PassiveType
    {
        Attack,
        MaxHealth,
        HealOnHit,
        ManaRegen,
        Thorns,
        Barrier,
        DodgeBoost
    }
    public class TestClass : MonoBehaviour
    {
        public void OnDeath()
        {
            EquipmentType type = Random.value < 0.5f ? EquipmentType.Weapon : EquipmentType.Armor;
            string iconAddress = type == EquipmentType.Weapon ? "Weapon/Icon" : "Armor/Icon";
            string borderAddress = type == EquipmentType.Weapon ? "Weapon/Border" : "Armor/Border";

            List<StatRange> statRanges = new()
            {
                new StatRange(StatType.Attack, 5, 10),
                new StatRange(StatType.MaxHealth, 10, 20)
            };

            List<PassiveEffect> passivePool = new()
            {
                new PassiveEffect(PassiveType.Thorns, 1, 2, 3, 4),
                new PassiveEffect(PassiveType.HealOnHit, 0.5f, 1f, 1.5f, 2f)
            };

            EquipmentFactory.CreateAndAddToInventory(type, iconAddress, borderAddress, statRanges, passivePool);
        }

    }
}