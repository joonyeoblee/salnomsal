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
            if (!_isParried)
            {
                Debug.Log("실패 처리됨");
                Parry.Life--;
                Destroy(gameObject);

            }
        }
    }
}