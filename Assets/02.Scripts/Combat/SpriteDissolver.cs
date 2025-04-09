using UnityEngine;
public class SpriteDissolver : MonoBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float speed = 1f;

    private float dissolveAmount;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            dissolveAmount += Time.deltaTime * speed;
            dissolveAmount = Mathf.Clamp01(dissolveAmount);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }
}
