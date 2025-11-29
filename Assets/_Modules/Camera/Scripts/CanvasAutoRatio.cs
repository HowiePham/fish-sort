using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [RequireComponent(typeof(CanvasScaler))]
    [DefaultExecutionOrder(-200)]
    public class CanvasAutoRatio : MonoBehaviour
    {
        private void Awake()
        {
            Calculate();
        }

        [Button]
        private void Calculate()
        {
            AspectRatio.GetAspectRatio(Screen.width, Screen.height, out Vector2Int ratio);

            var scaler = GetComponent<CanvasScaler>();

            if (ratio == new Vector2Int(3, 4))
            {
                scaler.referenceResolution = new Vector2(1536f, 2048f);
            }
            else if (ratio == new Vector2Int(4, 3))
            {
                scaler.referenceResolution = new Vector2(2048f, 1536f);
            }
            else
            {
                scaler.referenceResolution = new Vector2(1080f, 1920f);
            }
        }
    }
}