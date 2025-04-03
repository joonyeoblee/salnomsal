using System.Collections.Generic;
using UnityEngine;

namespace Jun{
    [DefaultExecutionOrder(-1)]
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;
        public List<Character> Monsters;
        public List<Character> PlayableCharacter;
        
        // 우선순위 큐
        
        // 너차례!
        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            // Monsters[1].StartTurn();
        }
    }

}
