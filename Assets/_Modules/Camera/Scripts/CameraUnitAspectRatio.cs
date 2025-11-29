using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CameraUnitAspectRatio : MonoBehaviour
    {
        [SerializeField] private float sizeDefault;
        [SerializeField] private float size21_9;
        [SerializeField] private float size20_9;
        [SerializeField] private float size18_9;
        [SerializeField] private float size18_39;
        [SerializeField] private float size16_9;
        [SerializeField] private float size4_3;

        private void Awake()
        {
            CalculateCameraUnit();
        }

        [Button]
        private void CalculateCameraUnit()
        {
            AspectRatio.GetAspectRatio(Screen.width, Screen.height, out Vector2Int ratio);
            Debug.Log($"Screen: {Screen.width}x{Screen.height}, Ratio: {ratio.x}:{ratio.y}");
            var cameraFit = GetComponent<CameraFit>();
            if (ratio == new Vector2Int(3, 4) || ratio == new Vector2Int(4, 3))
            {
                cameraFit.UnitsSize = this.size4_3;
            }
            else if (ratio == new Vector2Int(9, 21))
            {
                cameraFit.UnitsSize = this.size21_9;
            }
            else if (ratio == new Vector2Int(9, 18) || (float)ratio.x / ratio.y > 2f)
            {
                cameraFit.UnitsSize = this.size18_9;
            }
            else if (ratio == new Vector2Int(9, 16))
            {
                cameraFit.UnitsSize = this.size16_9;
            }
            else if (ratio == new Vector2Int(9, 20))
            {
                cameraFit.UnitsSize = this.size20_9;
            }
            else if (ratio == new Vector2Int(18, 39))
            {
                cameraFit.UnitsSize = this.size18_39;
            }
            else
            {
                cameraFit.UnitsSize = this.sizeDefault;
            }

            cameraFit.ComputeResolution();
        }
    }
}