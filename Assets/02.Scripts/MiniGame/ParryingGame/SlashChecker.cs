using UnityEngine;

namespace SeongIl
{
    public class SlashChecker : MonoBehaviour
    {
        public Parry Parry;
       public  bool _isParried;
        public float StartTime;

        public bool IsLastParry;
        public void ParriedCheck()
        {
            _isParried = true;
        }

        // 놓쳤을 경우 실패 호출용
        public void MissedCheck()
        {
            if (_isParried || Parry == null || Parry.AlreadyFail) return;

            Debug.Log("실패 처리됨");

            if (IsLastParry)
            {
                // 마지막 패링 놓침 → 바로 실패 처리
                Parry.Fail();
            } else
            {
                // 일반 패링 놓침 → 라이프 깎기
                Parry.Life--;
            }

            Destroy(gameObject);
        }
    }
}