using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public PlayableSkillSO SkillData;

    public abstract void UseSkill(PlayableCharacter user, ITargetable target);
}
