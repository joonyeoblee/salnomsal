using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jun.Map
{
    public class UI_MapGenerator : MonoBehaviour
    {
        [Header("UI References")]
        public RectTransform mapRoot;
        public GameObject mapNodePrefab; // UI_MapNode 프리팹
        public GameObject linePrefab; // UI_Line 프리팹

        [Header("Map Settings")]
        public int mapHeight = 15;
        public int mapWidth = 7;
        public float nodeSpacingX = 120f;
        public float nodeSpacingY = 180f;
        public long seed = 123456;
        public bool ascensionZero;
        

        void OnDisable()
        {
            Debug.Log("MapGenerator Disable");
            MapManager.Instance.OnMapNodeChanged -= () => gameObject.SetActive(false);
        }

        void Start()
        {
            Debug.Log("MapGenerator Start");
            MapManager.Instance.OnMapNodeChanged += () => gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void ButtonEvent()
        {
            // 기존 노드 및 선 제거
            foreach (Transform child in mapRoot)
            {
                Destroy(child.gameObject);
            }

            MapGenerator generator = new MapGenerator(seed);
            Map map = generator.GenerateMap(mapHeight, mapWidth, 3, ascensionZero);
            VisualizeMap(map);
        }


        void VisualizeMap(Map map)
        {
            Dictionary<MapNode, UI_MapNode> nodeUIMap = new Dictionary<MapNode, UI_MapNode>();

            foreach (MapNode node in map.Nodes)
            {
                Vector2 pos = new Vector2(node.X * nodeSpacingX, -node.Y * nodeSpacingY);

                GameObject go = Instantiate(mapNodePrefab, mapRoot);
                UI_MapNode nodeUI = go.GetComponent<UI_MapNode>();

                string label = node.Type.ToString().Substring(0, 1);
                Color color = GetColorByType(node.Type);

                nodeUI.Init(pos, label, color, node);
                nodeUIMap[node] = nodeUI;
            }

            foreach (MapNode node in map.Nodes)
            {
                if (!nodeUIMap.ContainsKey(node)) continue;

                Vector2 from = nodeUIMap[node].GetComponent<RectTransform>().anchoredPosition;

                foreach (MapNode child in node.Children)
                {
                    if (!nodeUIMap.ContainsKey(child)) continue;

                    Vector2 to = nodeUIMap[child].GetComponent<RectTransform>().anchoredPosition;
                    GameObject lineObj = Instantiate(linePrefab, mapRoot);
                    lineObj.GetComponent<UI_LineDrawer>().DrawLine(from, to);
                }
            }

            // 시작 노드 설정 (Y == -1인 노드 선택)
            MapNode startNode = map.Nodes.FirstOrDefault(n => n.Y == -1);

            if (startNode == null)
            {
                Debug.LogError("Start node is null! Y == -1 노드를 찾을 수 없습니다.");
                return;
            }

            MapManager.Instance.map = map;
            MapManager.Instance.SetCurrentNode(startNode);
        }


        Color GetColorByType(NodeType type)
        {
            return type switch
            {
                NodeType.Combat => Color.red,
                NodeType.Elite => new Color(0.7f, 0f, 0f),
                NodeType.Shop => Color.green,
                NodeType.Rest => Color.cyan,
                NodeType.Mystery => Color.yellow,
                NodeType.Boss => Color.magenta,
                NodeType.Null => Color.white,
                _ => Color.white
            };
        }
    }
}
