﻿using System.Collections.Generic;
using Jun;
using MoreMountains.Feedbacks;
using Portrait;
using UnityEngine;
using Enumerable = System.Linq.Enumerable;
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

    public MMF_Player Feel;

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
        UI_Battle.Instance.HideBattleUI();
        UI_Battle.Instance.HideEnemyHealthIndicator();
    }
    public void SpawnPlayer()
    {
        GameObject[] players = GameManager.Instance.Characters;
        CharacterStat[] characterStats = GameManager.Instance.CharacterStats;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                UI_Battle.Instance.PartyHealthIndicator.Initialize(null, i);
                UI_ChestInventory.Instance.Initialize(null, i);
                UI_ChestInventory.Instance.InitCharacterStats(null, i);
            }
            else
            {
                GameObject ch = Instantiate(players[i], new Vector3(SpawnPoint[i].position.x, SpawnPoint[i].position.y, 0), Quaternion.identity);
                Debug.Log(ch);
                PlayableCharacter player = ch.GetComponent<PlayableCharacter>();
                player.ApplyStat(characterStats[i].MaxHealth, characterStats[i].MaxMana, characterStats[i].AttackPower, characterStats[i].Speed);

                player.Index = i;
                PlayableCharacter.Add(player);
                UI_Battle.Instance.PartyHealthIndicator.Initialize(player, i);
                UI_ChestInventory.Instance.Initialize(player, i);
                UI_ChestInventory.Instance.InitCharacterStats(characterStats[i], i);
                // 장비
                // UI_ChestInventory.Instance.Armor[i].LoadEquiment(GameManager.Instance.PortraitItems[i].SaveData.Weapon.Id);
                UI_ChestInventory.Instance.Armor[i].LoadEquiment("3");
                UI_ChestInventory.Instance.Armor[i].LoadEquiment(GameManager.Instance.PortraitItems[i].SaveData.Armor.Id);
                
            }
        }
        UI_ChestInventory.Instance.gameObject.SetActive(false);
    }

    public void SpawnEnemy(EnemyCharacter enemyCharacter)
    {
        Debug.Log("적 스폰");
        Monsters.Add(enemyCharacter);
    }

    public void InitializeCombat()
    {
        TurnOrder.Clear();
        _isInputBlocked = false;

        UI_Battle.Instance.ShowPartyHealthIndicator();
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            if (character.IsAlive == false)
            {
                Debug.Log("전투 불가능 캐릭터");
                continue;
            }
            character.CurrentSpeed = character.BasicSpeed;
            UI_Battle.Instance.BattleUI[character.Index].Initialize(character);
            UI_Battle.Instance.PartyHealthIndicator.RefreshHealth(character);
            TurnOrder.Add(character);
        }

        UI_Battle.Instance.EnemyHealthIndicator.Initialize(Monsters);
        UI_Battle.Instance.ShowEnemyHealthIndicator();
        foreach (EnemyCharacter monster in Monsters)
        {
            monster.CurrentSpeed = monster.BasicSpeed;
            UI_Battle.Instance.EnemyHealthIndicator.RefreshHealth(monster);
            TurnOrder.Add(monster);
        }

        SetOrder();

        Debug.Log(TurnOrder.Count);
        SelectedSkill = SkillSlot.None;
        StartTurn();
    }

    public void SetOrder()
    {
        TurnOrder = Enumerable.ToList(Enumerable.OrderByDescending(TurnOrder, actor => actor.CurrentSpeed));
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
            UI_Battle.Instance.BattleUI[0].ResetButton();
            UI_Battle.Instance.BattleUI[1].ResetButton();
            UI_Battle.Instance.BattleUI[2].ResetButton();
            return;
        }

        if (slot == SelectedSkill)
        {
            Debug.Log("스킬 선택 취소");
            SelectedSkill = SkillSlot.None;
            UI_Battle.Instance.BattleUI[CurrentActor.Index].ResetButton();
            return;
        }

        if (CurrentActor.Skills[(int)slot].SkillCost > CurrentActor.Cost)
        {
            Debug.Log("마나가 부족합니다");
            UI_Battle.Instance.BattleUI[CurrentActor.Index].ResetButton();
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

        if (CurrentActor == null)
        {
            Debug.Log("상대턴 입니다.");
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

        for (int i = 0; i < PlayableCharacter.Count; ++i)
        {
            if (PlayableCharacter[i].IsAlive == false)
            {
                PlayableCharacter dead = PlayableCharacter[i];
                PlayableCharacter.Remove(dead);
                Destroy(dead.gameObject);
                --i;
            }
        }

        TurnOrder.Add(unit);
        SetNewTurn();
        SetOrder();

        DeActiveteAllOutline();

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
        OpenMapButton.SetActive(true);
        
        foreach (PlayableCharacter character in PlayableCharacter)
        {
            character.ResetAfterBattle();
        }

        ResetManager();
        UI_Battle.Instance.HideBattleUI();
        UI_Battle.Instance.HideEnemyHealthIndicator();

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
        UI_Battle.Instance.HideBattleUI();
        UI_Battle.Instance.HidePartyHealthIndicator();
        UI_Battle.Instance.HideEnemyHealthIndicator();
        MiniGameScenesManager.Instance.GoHome();
        // 컴뱃 매니저를 초기화 하고 씬매니저로 씬 전환
        return true;
    }
}
