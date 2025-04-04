using System.Collections.Generic;
using UnityEngine;

namespace Jun{
    [DefaultExecutionOrder(-1)]
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance;
        public List<EnemyCharacter> Monsters;
        public List<PlayableCharacter> PlayableCharacter;
        public PlayableCharacter CurrentActor;
        public EnemyCharacter[] SelectedEnemy;
        public int SpeedIncrementPerTurn;

        public List<ITurnActor> TurnOrder = new List<ITurnActor>();
        
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

        public void InitializeCombat()
        {

        }

        public void SetOrder()
        {
            TurnOrder.Sort((a, b) => b.CurrentSpeed.CompareTo(a.CurrentSpeed));
        }

        public void StartTurn()
        {
            ITurnActor unit = TurnOrder[0];
            unit.StartTurn();
            // UI에 CurrentCharacter에 대한 정보 표시 추가
        }

        public void EndTurn(ITurnActor unit)
        {
            unit.CurrentSpeed = unit.BasicSpeed;
            foreach (ITurnActor turnActor in TurnOrder)
            {
                turnActor.CurrentSpeed += SpeedIncrementPerTurn;
            }
            TurnOrder.Add(unit);
        }

        public void asdasd()
        {
            Monsters[0].StartTurn();
        }
    }

}
