using System;
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

        void OnEnable()
        {
            OnMapNodeChanged += SetRandomBackground;
        }

        void OnDisable()
        {
            OnMapNodeChanged -= SetRandomBackground;
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
    }
}
