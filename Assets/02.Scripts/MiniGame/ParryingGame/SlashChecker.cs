using System;
using UnityEngine;

namespace SeongIl
{
public class SlashChecker : MonoBehaviour
{
    public Action OnMissed;
    private bool _isParried = false;
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
            OnMissed?.Invoke();
            Destroy(gameObject);

        }
    }
}
}