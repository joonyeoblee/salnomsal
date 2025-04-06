using UnityEngine;

[CreateAssetMenu(fileName = "PlayableSkillSO", menuName = "Scriptable Objects/PlayableSkillSO")]
public class PlayableSkillSO : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public int SkillCost;
    public SkillType SkillType;
    public SkillRange SkillRange;
    public SkillTarget SkillTarget;
}
