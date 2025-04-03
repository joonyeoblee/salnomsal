using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jun{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;
        public List<Character> Monsters;
        public List<Character> PlayableCharacter;

        void Awake()
        {
            Instance = this;
        }
    }

}
