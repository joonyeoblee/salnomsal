using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jun{
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
            Monsters[0].StartTurn();
        }
    }

}
