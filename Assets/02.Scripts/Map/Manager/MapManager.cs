using UnityEngine;

namespace Jun.Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            } else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        public long seed = 123456;
        public bool ascensionZero;

        void Start()
        {
            MapGenerator generator = new MapGenerator(seed);
            Map map = generator.GenerateMap(15, 7, 6, ascensionZero);

            // TODO: map 시각화
            foreach (MapNode node in map.Nodes)
            {
                Debug.Log($"Node: {node.X}, {node.Y}, Type: {node.Type}");
            }
        }
    }
}
