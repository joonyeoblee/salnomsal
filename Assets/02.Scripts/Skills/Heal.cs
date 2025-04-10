using System;
using UnityEngine;

public class Heal : Skill
{
    public float HealAmount;

    public override void UseSkill(PlayableCharacter user, ITargetable target)
    {
        Vector3 position = target.Model.transform.position;

        DisplayText(position, HealAmount, FloatingTextType.Heal);
    }

    public void DisplayText(Vector3 position, float amount, FloatingTextType type)
    {
        string text = $"{Convert.ToInt32(amount)}";

        FloatingTextDisplay.Instance.ShowFloatingText(position, text, type);
    }
}
