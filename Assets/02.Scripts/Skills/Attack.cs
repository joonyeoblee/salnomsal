using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using Random = UnityEngine.Random;
public class Attack : Skill
{
    public float Multiplier;

    public override void UseSkill(PlayableCharacter caster, ITargetable target)
    {
        Damage damage = new Damage(caster.DamageType, caster.AttackPower * Multiplier, caster.gameObject);
        Vector3 position = target.Model.transform.position;

        if (Random.value < caster.CriticalChance)
        {
            damage.Value *= caster.CriticalDamage;

            MMF_Player player = CombatManager.Instance.Feel;

            MMF_CameraShake _cameraShake = player.GetFeedbackOfType<MMF_CameraShake>();
            _cameraShake.CameraShakeProperties.Duration = 0.5f;

            player.Initialization();
            player.PlayFeedbacks();

            _cameraShake.CameraShakeProperties.Duration = 1;
            player.Initialization();
            
            DisplayText(position, damage.Value, FloatingTextType.CriticalDamage);
        }
        else
        {
            DisplayText(position, damage.Value, FloatingTextType.Damage);
        }
        target.TakeDamage(damage);
    }

    public void DisplayText(Vector3 position, float amount, FloatingTextType type)
    {
        string text = string.Empty;
        if (type == FloatingTextType.Damage)
        {
            text = $"{Convert.ToInt32(amount)}";
        }
        else if (type == FloatingTextType.CriticalDamage)
        {
            text = $"Critical!\n{Convert.ToInt32(amount)}";
        }

        FloatingTextDisplay.Instance.ShowFloatingText(position, text, type);
    }
}
