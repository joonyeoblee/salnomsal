using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public List<EnemyCharacter> Monsters;
    public List<PlayableCharacter> PlayableCharacter;
    public PlayableCharacter CurrentActor;
    public SkillSlot SelectedSkill;
    public int SpeedIncrementPerTurn;

    public Transform[] SpawnPoint;

    private List<ITargetable> _target = new List<ITargetable>();

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

    void Start()
    {
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        List<GameObject> players = GameManager.Instance.Characters;
        for (int i = 0; i < players.Count; i++)
        {
            GameObject ch = Instantiate(players[i], new Vector3(SpawnPoint[i].position.x, SpawnPoint[i].position.y, 0), Quaternion.identity);
            PlayableCharacter.Add(ch.GetComponent<PlayableCharacter>());
        }
    }

    public void InitializeCombat()
    {
        // 전투가 시작되면 호출될 함수
        // PlayableCharacter = GameObject.FindGameObjectsWithTag("PlayableCharacter")
        //     .Select(obj => obj.GetComponent<PlayableCharacter>())
        //     .ToList(); // test

        Monsters = GameObject.FindGameObjectsWithTag("Enemy")
            .Select(obj => obj.GetComponent<EnemyCharacter>())
            .ToList(); // test

        foreach (PlayableCharacter character in PlayableCharacter)
        {
            character.CurrentSpeed = character.BasicSpeed;
            TurnOrder.Add(character);
        }

        foreach (EnemyCharacter monster in Monsters)
        {
            monster.CurrentSpeed = monster.BasicSpeed;
            TurnOrder.Add(monster);
        }

        SetOrder();
        Debug.Log(TurnOrder.Count);
        foreach (ITurnActor turnActor in TurnOrder)
        {
            Debug.Log(turnActor.CurrentSpeed);
        }
        SelectedSkill = SkillSlot.None;
        StartTurn();
    }

    public void SetOrder()
    {
        TurnOrder = TurnOrder.OrderByDescending(actor => actor.CurrentSpeed).ToList();
    }

    public void SetSelectedSkill(SkillSlot slot)
    {
        Debug.Log((int)slot);
        if (CurrentActor == null)
        {
            Debug.Log("상대턴 입니다.");
            return;
        }

        if (slot == SelectedSkill)
        {
            Debug.Log("스킬 선택 취소");
            SelectedSkill = SkillSlot.None;
            return;
        }

        if (CurrentActor.Skills[(int)slot].SkillCost > CurrentActor.Cost)
        {
            Debug.Log("마나가 부족합니다");
            SelectedSkill = SkillSlot.None;
            return;
        }
        SelectedSkill = slot;
    }

    public void SetTarget(ITargetable clicked)
    {
        if (SelectedSkill == SkillSlot.None)
        {
            Debug.Log("선택된 적 정보 출력");
            return;
        }

        PlayableSkillSO selectedSkillData = CurrentActor.Skills[(int)SelectedSkill].SkillData;

        if (selectedSkillData.SkillTarget != clicked.TargetType)
        {
            Debug.Log("올바르지 않은 타겟입니다");
            return;
        }

        if (selectedSkillData.SkillRange == SkillRange.Single)
        {
            _target.Add(clicked);
            Debug.Log("단일 타겟 스킬");
        }
        else if (selectedSkillData.SkillRange == SkillRange.Global)
        {
            Debug.Log("전체 타겟 스킬");
            foreach (ITargetable target in PlayableCharacter)
            {
                if (target.IsAlive && target.TargetType == selectedSkillData.SkillTarget)
                {
                    _target.Add(target);
                }
            }

            foreach (ITargetable target in Monsters)
            {
                if (target.IsAlive && target.TargetType == selectedSkillData.SkillTarget)
                {
                    _target.Add(target);
                }
            }   
        }
        CurrentActor.DoAction(SelectedSkill, _target);
    }

    public void StartTurn()
    {
        ITurnActor unit = TurnOrder[0];
        //GameObject gameObject = (unit as MonoBehaviour)?.gameObject;
        TurnOrder.RemoveAt(0);
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
        SetOrder();
        SetNewTurn();
        IsBattleEnd();
        IsGameOver();
        StartTurn();
    }

    public void SetNewTurn()
    {
        CurrentActor = null;
        SelectedSkill = SkillSlot.None;
        _target.Clear();
    }

    public void IsBattleEnd()
    {
        foreach (EnemyCharacter monster in Monsters)
        {
            if (monster.IsAlive)
            {
                return;
            }
        }
        Debug.Log("전투 종료, 승리");
        // 컴뱃 매니저를 초기화 하고 씬매니저로 씬 전환
    }

    public void IsGameOver()
    {
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            if (character.IsAlive)
            {
                return;
            }
        }
        Debug.Log("게임 오버");
        // 컴뱃 매니저를 초기화 하고 씬매니저로 씬 전환
    }
}
