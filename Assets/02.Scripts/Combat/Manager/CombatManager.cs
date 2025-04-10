using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public List<EnemyCharacter> Monsters;
    public List<PlayableCharacter> PlayableCharacter;
    public PlayableCharacter CurrentActor;

    public Transform PlayerAttackPosition;
    public Transform EnemyAttackPosition;
    
    public SkillSlot SelectedSkill;
    public int SpeedIncrementPerTurn;
    
    public GameObject OpenMapButton;

    public Transform[] SpawnPoint;

    private List<ITargetable> _target = new List<ITargetable>();
    public List<ITurnActor> TurnOrder = new List<ITurnActor>();

    private bool _isInputBlocked;

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

    public void ResetManager()
    {
        foreach (EnemyCharacter monster in Monsters)
        {
            Destroy(monster.gameObject);
        }
        Monsters.Clear();
        TurnOrder.Clear();
        CurrentActor = null;
        SelectedSkill = SkillSlot.None;
        _target.Clear();
    }

    void Start()
    {
        SpawnPlayer();

        // MapManager.Instance.OnMapNodeChanged += InitializeCombat;
        Debug.Log("Battle Scene Start");
    }

    void OnDisable()
    {
        // MapManager.Instance.OnMapNodeChanged -= InitializeCombat;
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

    public void SpawnEnemy(EnemyCharacter enemyCharacter)
    {
        Debug.Log("적 스폰");
        Monsters.Add(enemyCharacter);
    }

    public void InitializeCombat()
    {
        // 전투가 시작되면 호출될 함수
        // PlayableCharacter = GameObject.FindGameObjectsWithTag("PlayableCharacter")
        //     .Select(obj => obj.GetComponent<PlayableCharacter>())
        //     .ToList(); // test
        OpenMapButton.SetActive(false);
        TurnOrder.Clear();
        _isInputBlocked = false;

        foreach (PlayableCharacter character in PlayableCharacter)
        {
            if (character.IsAlive == false)
            {
                Debug.Log("전투 불가능 캐릭터");
                continue;
            }
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
        SelectedSkill = SkillSlot.None;
        StartTurn();
    }

    public void SetOrder()
    {
        TurnOrder = TurnOrder.OrderByDescending(actor => actor.CurrentSpeed).ToList();
    }

    public void SetSelectedSkill(SkillSlot slot)
    {
        if (_isInputBlocked)
        {
            return;
        }

        DeActiveteAllOutline();
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

        if (CurrentActor.Skills[(int)slot].SkillData.SkillTarget == TargetType.Enemy)
        {
            foreach (EnemyCharacter monster in Monsters)
            {
                monster.gameObject.GetComponent<TargetSelector>().ActivateOutlinable();
            }
        } else
        {
            if (CurrentActor.Skills[(int)slot].SkillData.SkillTarget == TargetType.Ally)
            {
                foreach (PlayableCharacter character in PlayableCharacter)
                {
                    character.gameObject.GetComponent<TargetSelector>().ActivateOutlinable();
                }
            }
        }
    }
    void DeActiveteAllOutline()
    {

        // 이미 켜져있는거 전부 끄기;
        foreach (EnemyCharacter monster in Monsters)
        {
            monster.gameObject.GetComponent<TargetSelector>().ChangeOutlineColor(Color.yellow);
            monster.gameObject.GetComponent<TargetSelector>().DeactivateOutlinable();
        }
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            character.gameObject.GetComponent<TargetSelector>().ChangeOutlineColor(Color.yellow);
            character.gameObject.GetComponent<TargetSelector>().DeactivateOutlinable();
        }
    }

    public void EnterTarget(TargetSelector targetSelector)
    {
        if (CurrentActor == null) return;
        if (SelectedSkill == SkillSlot.None) return;
        PlayableSkillSO selectedSkillData = CurrentActor.Skills[(int)SelectedSkill].SkillData;

        bool isEnemyTarget = selectedSkillData.SkillTarget == TargetType.Enemy;
        Color outlineColor = isEnemyTarget ? Color.red : Color.green;

        if (selectedSkillData.SkillRange == SkillRange.Single)
        {
            targetSelector.ChangeOutlineColor(outlineColor);
            Debug.Log("단일 타겟 스킬");
        } else if (selectedSkillData.SkillRange == SkillRange.Global)
        {
            if (isEnemyTarget)
            {
                foreach (EnemyCharacter monster in Monsters)
                    monster.GetComponent<TargetSelector>().ChangeOutlineColor(outlineColor);
            } else
            {
                foreach (PlayableCharacter character in PlayableCharacter)
                    character.GetComponent<TargetSelector>().ChangeOutlineColor(outlineColor);
            }
        }

    }

    public void ExitTarget()
    {
        if (CurrentActor == null) return;
        foreach (EnemyCharacter monster in Monsters)
        {
            monster.gameObject.GetComponent<TargetSelector>().ChangeOutlineColor(Color.yellow);
        }

        foreach (PlayableCharacter character in PlayableCharacter)
        {
            character.gameObject.GetComponent<TargetSelector>().ChangeOutlineColor(Color.yellow);
        }

    }
    public void SetTarget(ITargetable clicked)
    {
        if (_isInputBlocked)
        {
            return;
        }

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

        _isInputBlocked = true;
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

        DeActiveteAllOutline();
        
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
        DeActiveteAllOutline();
        unit.CurrentSpeed = unit.BasicSpeed;

        for (int i = 0; i < TurnOrder.Count; ++i)
        {
            if (TurnOrder[i] == null || TurnOrder[i].IsAlive == false)
            {
                TurnOrder.RemoveAt(i);
                --i;
                continue;
            }
            TurnOrder[i].CurrentSpeed += SpeedIncrementPerTurn;
        }

        TurnOrder.Add(unit);
        SetNewTurn();
        SetOrder();

        Debug.Log(TurnOrder.Count);
        if (IsBattleEnd() || IsGameOver())
        {
            return;
        }

        StartTurn(); // 전투가 종료됐다면 실행되면 안됨
    }

    public void SetNewTurn()
    {
        CurrentActor = null;
        _isInputBlocked = false;
        SelectedSkill = SkillSlot.None;
        _target.Clear();
    }

    public bool IsBattleEnd()
    {
        foreach (EnemyCharacter monster in Monsters)
        {
            if (monster.IsAlive)
            {
                return false;
            }
        }
        Debug.Log("전투 종료, 승리");
        
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            character.ResetAfterBattle();
        }

        ResetManager();
        OpenMapButton.SetActive(true);

        return true;
    }

    public bool IsGameOver()
    {
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            if (character.IsAlive)
            {
                return false;
            }
        }

        ResetManager();
        OpenMapButton.SetActive(true);
        Debug.Log("게임 오버");
        // 컴뱃 매니저를 초기화 하고 씬매니저로 씬 전환
        return true;
    }

    /*
    public void RemoveDeadCharacter()
    {
        for (int i = 0; i < PlayableCharacter.Count; i++)
        {
            if (PlayableCharacter[i].IsAlive == false)
            {
                Destroy(PlayableCharacter[i].gameObject);
                PlayableCharacter.RemoveAt(i);
                --i;
            }
        }
    }
    */
}
