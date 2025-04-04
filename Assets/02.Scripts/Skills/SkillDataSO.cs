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
    Global
}

public enum SkillTarget
{
    Ally,
    Enemy
}

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public int SkillCost;
    public float SkillMultiplier;
    public float Duration;
    public SkillType SkillType;
    public SkillRange SkillRange;
    public SkillTarget SkillTarget;
}

