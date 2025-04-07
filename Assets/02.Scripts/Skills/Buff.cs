using System;
using UnityEngine;

public enum BuffStatType
{
    AttackPower,
    CriticalChance,
    CriticalDamage,
    Taunt
}

public class Buff : Skill
{
    public int RemainingTurns;
    public float BuffMultiplier;
    public BuffStatType BuffStatType;

    private ITargetable _target;

    public override void UseSkill(PlayableCharacter user, ITargetable target)
    {
        Buff clone = Instantiate(this);
        clone._target = target;

        Debug.Log("버프 사용");
        clone._target.GetBuff(clone);
    }

    public void TickBuff()
    {
        if (RemainingTurns > 0)
        {
            --RemainingTurns;
        }
    }

    public void RemoveBuff()
    {
        if (RemainingTurns == 0)
        {
            _target.RemoveBuff(this);
        }
    }
}
