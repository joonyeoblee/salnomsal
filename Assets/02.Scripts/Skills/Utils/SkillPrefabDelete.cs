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
        GameObject parent = transform.parent != null ? transform.parent.gameObject : null;

        Destroy(gameObject); // 자식 먼저
        if (parent != null)
        {
            Destroy(parent); // 그다음 부모
        }
        
    }
}
