
using System;
using UnityEngine;

using UnityEngine.UI;
namespace SeongIl
{


    public class MagicSequence : MonoBehaviour
    {
        public GameObject Enemy;
        public Image MainMagicCircle;
        public Image SubMagicCircle;

        private void Update()
        {
            MainMagicCircle.rectTransform.Rotate(0,0, -5f * Time.deltaTime);
            SubMagicCircle.rectTransform.Rotate(0,0, 10f * Time.deltaTime);
        }
    }
}