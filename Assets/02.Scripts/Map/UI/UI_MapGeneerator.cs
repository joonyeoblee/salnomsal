using System.Collections.Generic;
using UnityEngine;

namespace Jun.Map
{
    public class MapGeneratorUI : MonoBehaviour
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

        void Start()
        {
            MapGenerator generator = new MapGenerator(seed);
            Map map = generator.GenerateMap(mapHeight, mapWidth, 6, ascensionZero);
            VisualizeMap(map);
        }

        void VisualizeMap(Map map)
        {
            Dictionary<MapNode, UI_MapNode> nodeUIMap = new Dictionary<MapNode, UI_MapNode>();

            // 1. 노드 배치
            foreach (MapNode node in map.Nodes)
            {
                Vector2 pos = new Vector2(
                    node.X * nodeSpacingX,
                    -node.Y * nodeSpacingY
                );

                GameObject go = Instantiate(mapNodePrefab, mapRoot);
                UI_MapNode nodeUI = go.GetComponent<UI_MapNode>();

                string label = node.Type.ToString().Substring(0, 1);
                Color color = GetColorByType(node.Type);

                nodeUI.Init(pos, label, color);
                nodeUIMap[node] = nodeUI;
            }

            // 2. 연결선 그리기
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
                _ => Color.white
            };
        }
    }
}
