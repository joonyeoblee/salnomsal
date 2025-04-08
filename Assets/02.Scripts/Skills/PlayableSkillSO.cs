using UnityEngine;

[CreateAssetMenu(fileName = "PlayableSkillSO", menuName = "Scriptable Objects/PlayableSkillSO")]
public class PlayableSkillSO : ScriptableObject
{
    public SkillType SkillType;
    public SkillRange SkillRange;
    public TargetType SkillTarget;
}
