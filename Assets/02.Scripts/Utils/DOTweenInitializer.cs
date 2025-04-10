using DG.Tweening;
using UnityEngine;
public class DOTweenInitializer : MonoBehaviour
{
    void Awake()
    {

        DOTween.Init(false); // SafeMode 끔

    }
}
