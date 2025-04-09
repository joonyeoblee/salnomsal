using MoreMountains.Feedbacks;
using UnityEngine;

public class FloatingTextTest : MonoBehaviour
{
    private MMF_Player _myPlayer;

    private void Start()
    {
        _myPlayer = GetComponent<MMF_Player>();
    }

    public void OnClickButton()
    {
        _myPlayer.PlayFeedbacks(this.transform.position, 123f);
    }
}
