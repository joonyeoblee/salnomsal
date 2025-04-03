using UnityEngine;


namespace PlayableCharacters
{
    // 플레이어 캐릭터를 나타내는 클래스입니다.
    // Character 클래스를 상속받아 구현합니다.
    // 플레이어 캐릭터는 공격, 스킬 사용, 사망 등의 기능을 가집니다.
    // 플레이어 캐릭터는 Damage를 받아 체력을 감소시키고, 체력이 0 이하가 되면 사망합니다.
    public class PlayableCharacter : Character
    {


        protected override void Attack(Character enemy)
        {
            // enemy.TakeDamage(new Damage(DamageType, , this.gameObject));
            Debug.Log("Attack");
        }

        protected override void Skill1(Character enemy)
        {
            Debug.Log("Skill1");
        }

        protected override void Skill2(Character enemy)
        {
            Debug.Log("Skill2");
        }

        protected override void Death(DamageType type)
        {
            Debug.Log("Death");
        }

        public override void TakeDamage(Damage damage)
        {
            _health -= damage.Value;
            if (_health <= 0)
            {
                Death(damage.Type);
            }
            else
            {
                Debug.Log($"Took {damage.Value} damage from {damage.DamageFrom.name}. Remaining health: {_health}");
            }
        }
    }
}
