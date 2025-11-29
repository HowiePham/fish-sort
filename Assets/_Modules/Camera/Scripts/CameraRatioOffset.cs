using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class CameraRatioOffset : MonoBehaviour
    {
        [SerializeField] private float offset21_9;
        [SerializeField] private float offset18_9;
        [SerializeField] private float offset16_9;
        [SerializeField] private float offset4_3;

        private void Awake()
        {
            CalculateCameraUnitOffset();
        }

        [Button]
        private void CalculateCameraUnitOffset()
        {
            AspectRatio.GetAspectRatio(Screen.width, Screen.height, out Vector2Int ratio);
            var cameraFit = GetComponent<CameraFit>();
            if (ratio == new Vector2Int(3, 4) || ratio == new Vector2Int(4, 3))
            {
                cameraFit.offsetUnit = this.offset4_3;
            }
            else if (ratio == new Vector2Int(21, 9))
            {
                cameraFit.offsetUnit = this.offset21_9;
            }
            else if (ratio == new Vector2Int(18, 9) || (float) ratio.x / ratio.y > 2f)
            {
                cameraFit.offsetUnit = this.offset18_9;
            }
            else if (ratio == new Vector2Int(16, 9))
            {
                cameraFit.offsetUnit = this.offset16_9;
            }
            else
            {
                cameraFit.offsetUnit = 0f;
            }

            cameraFit.ComputeResolution();
        }
    }
}