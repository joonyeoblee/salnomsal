using System.Collections.Generic;
using UnityEngine;

namespace Jun{
    [DefaultExecutionOrder(-1)]
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;
        public List<EnemyCharacter> Monsters;
        public List<Character> PlayableCharacter;

        // 우선순위 큐
        
        // 너차례!
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void asdasd()
        {
            Monsters[0].StartTurn();
        }
        
    }

}
