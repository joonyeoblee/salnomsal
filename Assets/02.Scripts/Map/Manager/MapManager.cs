using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Jun.Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;

        public long seed = 123456;
        public bool ascensionZero;

        public Map map; // 현재 맵
        public MapNode currentNode; // 현재 위치 노드

        public UI_MapGenerator mapGenerator;
        public Image BackGround;

        public Sprite[] BackGroundSprites;

        public Sprite BossBackGround;

        public Action OnMapNodeChanged;

        public AudioClip[] AudioClips;
        public AudioSource AudioSource;

        public GameObject MapOpenButton;
 
        void OnEnable()
        {
            OnMapNodeChanged += SetRandomBackground;
            OnMapNodeChanged += SetRandomBGM;
        }

        void OnDisable()
        {
            OnMapNodeChanged -= SetRandomBackground;
            OnMapNodeChanged -= SetRandomBGM;
        }
        void Awake()
        {
            Debug.Log("MapGenerator Awake");
            if (Instance == null)
            {
                Instance = this;
     
            } else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            mapGenerator.ButtonEvent();
            mapGenerator.gameObject.SetActive(false);
            OnMapNodeChanged += MapOpenButton.GetComponent<UI_Selector>().HideButton;
        }

        public void SetCurrentNode(MapNode node)       
        {
            currentNode = node;

            // 자식 중에서 '이 노드를 부모로 갖고 있는' 노드만 활성화
            foreach (UI_MapNode uiNode in FindObjectsOfType<UI_MapNode>())
            {
                MapNode target = uiNode.Source;

                bool isNext =
                    currentNode.Children.Contains(target) &&
                    target.Parents.Contains(currentNode);

                uiNode.SetInteractable(isNext);
            }
        }

        public void SetRandomBackground()
        {
            if (currentNode.Type == NodeType.Boss)
            {
                BackGround.sprite = BossBackGround;
                return;
            }
            if (BackGroundSprites == null || BackGroundSprites.Length == 0)
            {
                Debug.LogWarning("BackGroundSprites 배열이 비어있습니다.");
                return;
            }

            int randomIndex = Random.Range(0, BackGroundSprites.Length);
            BackGround.sprite = BackGroundSprites[randomIndex];
        }
        
        public void SetRandomBGM()
        {
            if (AudioClips == null || AudioClips.Length == 0)
            {
                Debug.LogWarning("AudioClips 배열이 비어있습니다.");
                return;
            }

            // 현재 재생 중인 클립을 제외한 리스트 생성
            var availableClips = new List<AudioClip>();

            foreach (var clip in AudioClips)
            {
                if (clip != null && clip != AudioSource.clip)
                {
                    availableClips.Add(clip);
                }
            }

            if (availableClips.Count == 0)
            {
                Debug.LogWarning("현재 재생 중인 클립 외에 선택 가능한 클립이 없습니다.");
                return;
            }

            // 랜덤한 클립 선택 후 재생
            AudioClip randomClip = availableClips[Random.Range(0, availableClips.Count)];
            AudioSource.clip = randomClip;
            AudioSource.Play();
        }

    }
}
