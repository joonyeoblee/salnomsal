namespace Jun.Monster
{
    public class Ogre : MonsterBase
    {
        public bool IsMyTurn;

        Character _target;
        void Update()
        {
            if (IsMyTurn)
            {
                Attack();
                IsMyTurn = false;
            }
        }

        protected override void Attack()
        {
            base.Attack();
            Damage _damage = new Damage(DamageType.Melee, AttackPower, gameObject);
            _target.TakeDamage(_damage);
        }
        public override void StartTurn()
        {
            _target = GetTarget();
        }
    }
}
