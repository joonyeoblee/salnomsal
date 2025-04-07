using DG.Tweening;
using UnityEngine;

public class AvoidCameraMove : MonoBehaviour
{
    public Camera AvoidCamera;
    public GameObject Player;
    public float followStrength = 0.1f;

    private Vector3 _startOffset;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        AvoidCamera.DOOrthoSize(4f, 0.2f);
        // 카메라 기준 offset 저장 (자식 구조로 되어 있으니 localPosition 기준)
        _startOffset = transform.position - Player.transform.position;
    }

    private void Update()
    {
        Vector3 targetPos = Player.transform.position/10 + _startOffset + new Vector3(Player.transform.position.x, Player.transform.position.y, 0f) * followStrength;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 1f);
    }
}
