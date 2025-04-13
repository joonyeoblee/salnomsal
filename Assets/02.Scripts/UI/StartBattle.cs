using UnityEngine;

namespace SeongIl.UI
{
    public class StartBattle : MonoBehaviour
    {
        [SerializeField] SceneTransition _sceneTransition;
        public void EndAnimation()
        {
            _sceneTransition.DeActiveBattle();
            CombatManager.Instance.InitializeCombat();
        }
    }
}
