using UnityEngine;
[CreateAssetMenu(fileName = "PortraitSO", menuName = "Scriptable Objects/PortraitSO")]
public class PortraitSO : ScriptableObject
{
    public float MaxHealth;
    public float MaxMana;
    public float AttackPower;
    public int Speed;
    public GameObject Character;
    public Sprite Icon;
   
}
