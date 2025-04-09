using UnityEngine;

public class Attack : Skill
{
    public float Multiplier;

    public override void UseSkill(PlayableCharacter caster, ITargetable target)
    {
        // Implement the attack logic here
        Debug.Log($"{caster.CharacterName} used {SkillName}");
        // Example: target.TakeDamage(SkillData.SkillMultiplier);
        Damage damage = new Damage(caster.DamageType, caster.AttackPower * Multiplier, caster.gameObject);
        
        target.TakeDamage(damage);
    }
}
