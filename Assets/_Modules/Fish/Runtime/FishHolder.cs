using System;
using UnityEngine;

public class FishHolder : MonoBehaviour
{
    [SerializeField] private int fishTypeNumber;
    [SerializeField] private bool isOccupied;

    public Action OnFishMovingIn;
    public bool IsOccupied => this.isOccupied;
    public int FishTypeNumber => this.fishTypeNumber;

    public void OccupyHolder(int typeNumber)
    {
        this.isOccupied = true;
        this.fishTypeNumber = typeNumber;
        OnFishMovingIn?.Invoke();
    }

    public void LeaveHolder()
    {
        this.fishTypeNumber = -1;
        this.isOccupied = false;
    }
}