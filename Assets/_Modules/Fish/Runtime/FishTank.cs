using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    [SerializeField] private FishHolder[] fishHolders;
    [SerializeField] private bool isCompleted;

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

#if UNITY_EDITOR
    [Button]
    private void GetFishHolders()
    {
        this.fishHolders = GetComponentsInChildren<FishHolder>();
    }
#endif
}