using UnityEngine;

namespace Jun.Test
{
    public class TestPlayable : Character
    {
        protected override void Attack()
        {
            
        }
        protected override void Skill1()
        {
            
        }
        protected override void Skill2()
        {
            
        }
        protected override void Death(DamageType type)
        {
            
        }
        public override void TakeDamage(Damage damage)
        {
            _health -= damage.Value;
            Debug.Log(_health + $"{gameObject.name} - {damage.Value}");
        }
    }
}