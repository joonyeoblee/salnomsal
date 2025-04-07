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

    public void MissedCheck()
    {
        Debug.Log("CheckMiss 호출됨");
        if (!_isParried)
        {
            Debug.Log("실패 처리됨");
            OnMissed?.Invoke();
        }
        Destroy(gameObject);
    }
}
}