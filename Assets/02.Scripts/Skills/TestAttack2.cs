using Jun;
using UnityEngine;

public class TestAttack2 : Skill
{
    public float Multiplier = 1.5f;

    public override void UseSkill(PlayableCharacter caster, ITargetable target)
    {

        // Implement the attack logic here
        Debug.Log($"{caster.CharacterName} used {SkillData.SkillName}");
        // Example: target.TakeDamage(SkillData.SkillMultiplier);
        Damage damage = new Damage(caster.DamageType, caster.AttackPower * Multiplier, caster.gameObject);
        target.TakeDamage(damage);
    }
}
