using UnityEngine;

public class TestHeal : Skill
{
    public float HealAmount = 30f;

    public override void UseSkill(PlayableCharacter user, ITargetable target)
    {
        // Implement the heal logic here
        Debug.Log($"{user.CharacterName} used {SkillData.SkillName}");
        // Example: target.Heal(SkillData.SkillMultiplier);
        target.GetHeal(HealAmount);
    }
}
