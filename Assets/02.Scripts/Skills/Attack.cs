using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Attack : Skill
{
    public float Multiplier;

    public override void UseSkill(PlayableCharacter caster, ITargetable target)
    {
        MonoBehaviour monoBehaviour = target as MonoBehaviour;
        Vector3 position = monoBehaviour.transform.Find("Model").transform.position;
        Debug.Log(position);
        Damage damage = new Damage(caster.DamageType, caster.AttackPower * Multiplier, caster.gameObject);

        if (UnityEngine.Random.value < caster.CriticalChance)
        {
            damage.Value *= caster.CriticalDamage;
            DisplayText(position, damage.Value, FloatingTextType.CriticalDamage);
        }
        else
        {
            DisplayText(position, damage.Value, FloatingTextType.Damage);
        }
        target.TakeDamage(damage);
    }

    public void DisplayText(Vector3 position, float amount, FloatingTextType type)
    {
        string text = string.Empty;
        if (type == FloatingTextType.Damage)
        {
            text = string.Format($"{Convert.ToInt32(amount)}");
        }
        else if (type == FloatingTextType.CriticalDamage)
        {
            text = string.Format($"Critical!\n{Convert.ToInt32(amount)}");
        }

        FloatingTextDisplay.Instance.ShowFloatingText(position, text, type);
    }
}
