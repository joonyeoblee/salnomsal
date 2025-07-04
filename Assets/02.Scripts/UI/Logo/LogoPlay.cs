using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LogoPlay : MonoBehaviour
{
    public Image Logo;
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        LogoPlaySequence();
    }

    private void LogoPlaySequence()
    {
        Logo.DOFade(1f, 1f).OnComplete(() =>
        {
            _animator.SetTrigger("Start");
        });
    }

    public void AnimationFinished()
    {
        Logo.DOFade(0, 1f).OnComplete(() =>
        {
            SceneManager.LoadScene("SalnomSalTitleMenu");
        });
    }
    
}
