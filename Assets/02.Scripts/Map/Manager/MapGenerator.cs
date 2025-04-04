using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Jun.Map
{
    public enum NodeType
    {
        Combat, Elite, Shop, Rest, Mystery, Boss, Null
    }

    public class MapNode
    {
        public int X;
        public int Y;
        public NodeType Type;
        public List<MapNode> Parents = new List<MapNode>();
        public List<MapNode> Children = new List<MapNode>();
    }

    public class Map
    {
        public List<MapNode> Nodes = new List<MapNode>();
        public int Act;
    }

    public class MapGenerator
    {
        void AssignRoomTypes(Map map)
        {
            foreach (MapNode node in map.Nodes)
            {
                if (node.Y == 0 || node.Type == NodeType.Boss) continue;

                float roll = (float)rng.NextDouble();
                if (roll < 0.08f)
                    node.Type = NodeType.Elite;
                else if (roll < 0.22f)
                    node.Type = NodeType.Mystery;
                else if (roll < 0.34f)
                    node.Type = NodeType.Rest;
                else if (roll < 0.39f)
                    node.Type = NodeType.Shop;
                else
                    node.Type = NodeType.Combat;
            }
        }
        readonly Random rng;
        bool bossPlaced;

        public MapGenerator(long seed)
        {
            rng = new Random((int)(seed % int.MaxValue));
        }

        public Map GenerateMap(int height, int width, int pathCount, bool ascensionZero)
        {
            Map map = new Map();
            MapNode[,] grid = new MapNode[height, width];
            HashSet<(int y, int x)> usedPositions = new HashSet<(int y, int x)>();
            int centerX = Mathf.FloorToInt(width / 2);

            // 1. 격자 전체 생성 (모든 위치에 노드 미리 생성)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 마지막 줄에서는 중앙 X 위치만 유지
                    if (y == height - 1 && x != centerX) continue;

                    NodeType type = GetNodeTypeByLayer(y, height, ref bossPlaced);
                    if (type == NodeType.Null) continue;

                    MapNode node = new MapNode
                    {
                        X = x,
                        Y = y,
                        Type = type
                    };
                    grid[y, x] = node;
                }
            }

            List<int> validStartXs = Enumerable.Range(width / 2, Mathf.Max(1, width - width / 2)).Where(x => x < width && grid[0, x] != null).ToList();

            // if (validStartXs.Count == 0)
            // {
            //     // fallback: 중앙 X에 강제 생성
            //     int fallbackX = centerX;
            //     MapNode fallbackNode = new MapNode
            //     {
            //         X = fallbackX,
            //         Y = 0,
            //         Type = NodeType.Combat
            //     };
            //     grid[0, fallbackX] = fallbackNode;
            //     validStartXs.Add(fallbackX);
            // }

            List<int> startXs = validStartXs.OrderBy(_ => rng.Next()).Take(Mathf.Min(pathCount, validStartXs.Count)).ToList();

            for (int i = 0; i < startXs.Count; i++)
            {
                int x = startXs[i];
                int y = 0;

                MapNode start = grid[y, x];
                usedPositions.Add((y, x));

                // 경로 따라 올라가며 연결
                for (int step = 0; step < height - 1 && y + 1 < height; step++)

                {
                    int nextY = y + 1;
                    List<int> candidates = new List<int>();
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        int nx = x + dx;
                        if (nx >= 0 && nx < width && grid[nextY, nx] != null && !usedPositions.Contains((nextY, nx)))
                        {
                            candidates.Add(nx);
                        }
                    }

                    if (candidates.Count == 0) break;

                    int nextX = candidates[rng.Next(candidates.Count)];
                    usedPositions.Add((nextY, nextX));

                    MapNode from = grid[y, x];
                    MapNode to = grid[nextY, nextX];

                    from.Children.Add(to);
                    to.Parents.Add(from);

                    x = nextX;
                    y = nextY;
                }
            }

            // 마지막 줄 바로 전 노드들이 마지막 줄 노드와 연결되도록 보장
            int lastY = height - 1;
            int prevY = lastY - 1;
            for (int x = 0; x < width; x++)
            {
                if (grid[prevY, x] != null && grid[lastY, x] != null && usedPositions.Contains((prevY, x)) && usedPositions.Contains((lastY, x)))
                {
                    MapNode from = grid[prevY, x];
                    MapNode to = grid[lastY, x];
                    if (!from.Children.Contains(to))
                    {
                        from.Children.Add(to);
                        to.Parents.Add(from);
                    }
                }
            }

            // 3. 사용된 노드 중 Boss까지 연결된 노드만 추가
            MapNode bossNode = grid[height - 1, centerX];
            HashSet<MapNode> reachable = new HashSet<MapNode>();

            void Traverse(MapNode node)
            {
                if (node == null || reachable.Contains(node)) return;
                reachable.Add(node);
                foreach (MapNode parent in node.Parents)
                {
                    Traverse(parent);
                }
            }

            Traverse(bossNode);

            Dictionary<(int y, int x), MapNode> allUsed = usedPositions.ToDictionary(pos => pos, pos => grid[pos.y, pos.x]);
            foreach (MapNode node in allUsed.Values)
            {
                if (node.Type == NodeType.Boss)
                {
                    Traverse(node);
                }
            }

            foreach (MapNode node in allUsed.Values)
            {
                if (reachable.Contains(node))
                {
                    map.Nodes.Add(node);
                }
            }

            AssignRoomTypes(map);
            AssignRoomTypes(map);
            Debug.Log($"생성된 총 노드 수: {map.Nodes.Count}");
            return map;
        }

        NodeType GetNodeTypeByLayer(int y, int height, ref bool bossPlaced)
        {
            if (y == 0) return NodeType.Combat; // 첫 줄은 Combat만
            if (y == height - 1)
            {
                if (!bossPlaced)
                {
                    bossPlaced = true;
                    return NodeType.Boss; // 마지막 줄 단 하나만 Boss
                }
                return NodeType.Null;
            }
            return NodeType.Combat; // 초기에는 Combat으로 고정 생성
        }
    }
}
