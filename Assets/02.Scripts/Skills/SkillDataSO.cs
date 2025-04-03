using UnityEngine;

public enum SkillType
{
    Attack,
    Buff,
    Debuff,
    Heal
}

public enum SkillRange
{
    Single,
    Global,
}

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public int SkillCost;
    public float SkillMultiplier;
    public SkillType SkillType;
    public SkillRange SkillRange;
}

