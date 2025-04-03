using UnityEngine;


namespace PlayableCharacters
{
    // �÷��̾� ĳ���͸� ��Ÿ���� Ŭ�����Դϴ�.
    // Character Ŭ������ ��ӹ޾� �����մϴ�.
    // �÷��̾� ĳ���ʹ� ����, ��ų ���, ��� ���� ����� �����ϴ�.
    // �÷��̾� ĳ���ʹ� Damage�� �޾� ü���� ���ҽ�Ű��, ü���� 0 ���ϰ� �Ǹ� ����մϴ�.
    public class PlayableCharacter : Character
    {


        protected override void Attack()
        {
            Debug.Log("Attack");
        }

        protected override void Skill1()
        {
            Debug.Log("Skill1");
        }

        protected override void Skill2()
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
