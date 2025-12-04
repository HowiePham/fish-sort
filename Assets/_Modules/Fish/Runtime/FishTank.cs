using Cysharp.Threading.Tasks;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField] private ParticleSystem finishFx;
    [SerializeField] private Transform entryPoint;
    [SerializeField] private Transform waterPoint;
    [SerializeField] private FishHolder[] fishHolders;
    [SerializeField] private bool isCompleted;
    public Vector3 EntryPos => this.entryPoint.position;
    public Vector3 WaterPos => this.waterPoint.position;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (FishHolder fishHolder in this.fishHolders)
        {
            fishHolder.OnFishMovingIn += CheckHolders;
        }
    }

    private void CheckHolders()
    {
        int fishTypeNumber = this.fishHolders[0].FishTypeNumber;
        foreach (FishHolder fishHolder in this.fishHolders)
        {
            if (!fishHolder.IsOccupied)
            {
                return;
            }

            int typeNumber = fishHolder.FishTypeNumber;
            if (fishTypeNumber != typeNumber)
            {
                return;
            }
        }

        FinishFishTank();
    }

    private async UniTask FinishFishTank()
    {
        await UniTask.WaitForSeconds(1);

        this.finishFx.Play();
    }

    public FishHolder SelectFishHolder(LeanFinger finger)
    {
        var minDistance = float.MaxValue;
        Vector3 fingerPos = finger.GetWorldPosition(10);
        FishHolder selectedHolder = null;

        foreach (FishHolder holder in this.fishHolders)
        {
            if (!holder.CanInteract(finger))
            {
                continue;
            }

            float distance = Vector3.Distance(fingerPos, holder.transform.position);
            if (distance < minDistance)
            {
                selectedHolder = holder;
                minDistance = distance;
            }
        }

        return selectedHolder;
    }

    public FishHolder OccupyEmptyHolder(int fishType, Transform fishTransform)
    {
        foreach (FishHolder fishHolder in this.fishHolders)
        {
            if (!fishHolder.IsOccupied)
            {
                fishHolder.OccupyHolder(fishType, fishTransform);
                return fishHolder;
            }
        }

        return null;
    }

    public bool IsFull()
    {
        foreach (FishHolder fishHolder in this.fishHolders)
        {
            if (!fishHolder.IsOccupied)
            {
                return false;
            }
        }

        return true;
    }

#if UNITY_EDITOR
    [Button]
    private void GetFishHolders()
    {
        this.fishHolders = GetComponentsInChildren<FishHolder>();
    }
#endif
}