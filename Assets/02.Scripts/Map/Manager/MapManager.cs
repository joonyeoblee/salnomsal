using UnityEngine;

namespace Jun.Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager instance;

        public long seed = 123456;
        public bool ascensionZero;

        public Map map; // 현재 맵
        public MapNode currentNode; // 현재 위치 노드

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
        
    }
}
