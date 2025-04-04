using System;
using System.Collections.Generic;
using System.Linq;

namespace Jun.Map
{
    public enum NodeType
    {
        Combat, Elite, Shop, Rest, Mystery, Boss
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
        readonly Random rng;

        public MapGenerator(long seed)
        {
            rng = new Random((int)(seed % int.MaxValue)); // Unity Random과 다르니 System.Random 사용
        }

        public Map GenerateMap(int height, int width, int pathDensity, bool ascensionZero)
        {
            Map map = new Map();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 랜덤 확률로 노드를 만들거나 말거나 결정
                    if (rng.NextDouble() < 0.5)
                    {
                        MapNode node = new MapNode
                        {
                            X = x,
                            Y = y,
                            Type = GetRandomNodeType()
                        };
                        map.Nodes.Add(node);
                    }
                }
            }

            // 노드 연결 생성
            for (int y = 0; y < height - 1; y++)
            {
                List<MapNode> currentLayer = map.Nodes.Where(n => n.Y == y).ToList();
                List<MapNode> nextLayer = map.Nodes.Where(n => n.Y == y + 1).ToList();

                foreach (MapNode node in currentLayer)
                {
                    // 연결 개수 제한 또는 확률 기반
                    int connectionCount = rng.Next(1, 3); // 1~2개 연결

                    IEnumerable<MapNode> targets = nextLayer
                        .OrderBy(_ => rng.Next())
                        .Take(connectionCount);

                    foreach (MapNode target in targets)
                    {
                        node.Children.Add(target);
                        target.Parents.Add(node);
                    }
                }
            }
            return map;
        }

        NodeType GetRandomNodeType()
        {
            int roll = rng.Next(100);
            if (roll < 50) return NodeType.Combat;
            if (roll < 70) return NodeType.Mystery;
            if (roll < 80) return NodeType.Rest;
            if (roll < 90) return NodeType.Shop;
            return NodeType.Elite;
        }
    }
}
