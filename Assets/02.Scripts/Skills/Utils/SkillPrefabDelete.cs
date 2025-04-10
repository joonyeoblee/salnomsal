using UnityEngine;
public class SkillPrefabDelete : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Skill");
    }
    
    public void DeleteThis()
    {
        Destroy(gameObject);
    }
}
