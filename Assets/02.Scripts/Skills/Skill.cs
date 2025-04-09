using UnityEngine;
public abstract class Skill : MonoBehaviour
{
    public PlayableSkillSO SkillData;
    public string SkillName;
    public string SkillDescription;
    public int SkillCost;
    
    public abstract void UseSkill(PlayableCharacter user, ITargetable target);
}
