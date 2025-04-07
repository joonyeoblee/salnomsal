using UnityEngine;

public interface ITargetable
{
    public void TakeDamage(Damage damage);
    public void GetBuff(float amount); // 나중에 버프 인스턴스 추가될 수 있음
    public void GetHeal(float amount); // 나중에 스킬 인스턴스 변경될 수 있음

    public bool IsAlive { get; }
    public TargetType TargetType { get; set; }
}
