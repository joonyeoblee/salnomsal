using UnityEngine;

public interface ITargetable
{
    public void TakeDamage(Damage damage);
    public void GetBuff(Buff buff);
    public void RemoveBuff(Buff buff);
    public void GetHeal(float amount); // 나중에 스킬 인스턴스 변경될 수 있음

    public bool IsAlive { get; }
    public TargetType TargetType { get; set; }
    public GameObject Model { get; }
}
