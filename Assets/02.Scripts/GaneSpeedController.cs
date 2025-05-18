using UnityEngine;
public class GameSpeedController : MonoBehaviour
{
    [SerializeField] float fastSpeed = 1.5f;
    float defaultFixedDeltaTime;

    void Awake()
    {
        // 기본 fixedDeltaTime 저장
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Start()
    {
        SetGameSpeed(fastSpeed); // 시작 시 빠른 속도로 설정하고 싶다면
    }

    /// <summary>
    ///     게임 속도를 설정합니다.
    /// </summary>
    public void SetGameSpeed(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * scale;
    }

    /// <summary>
    ///     게임 속도를 기본값(1.0)으로 되돌립니다.
    /// </summary>
    public void ResetGameSpeed()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
