using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

public class FishTank : MonoBehaviour
{
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
                this.isCompleted = false;
                return;
            }

            int typeNumber = fishHolder.FishTypeNumber;
            if (fishTypeNumber != typeNumber)
            {
                this.isCompleted = false;
                return;
            }
        }

        this.isCompleted = true;
    }

    public FishHolder SelectFishHolder(LeanFinger finger)
    {
        foreach (FishHolder holder in this.fishHolders)
        {
            if (holder.CanInteract(finger))
            {
                return holder;
            }
        }

        return null;
    }

    public Vector3 OccupyEmptyHolder(int fishType)
    {
        foreach (FishHolder fishHolder in this.fishHolders)
        {
            if (!fishHolder.IsOccupied)
            {
                fishHolder.OccupyHolder(fishType);
                return fishHolder.transform.position;
            }
        }

        return Vector3.zero;
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