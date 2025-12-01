using System;
using Lean.Touch;
using UnityEngine;

public class FishHolder : MonoBehaviour
{
    [SerializeField] private int fishTypeNumber;
    [SerializeField] private float interactingSize;
    [SerializeField] private bool isOccupied;
    
    public Vector3 Position => this.transform.position;

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

    public bool CanInteract(LeanFinger finger)
    {
        Vector3 fingerPos = finger.GetWorldPosition(10);
        float distance = Vector3.Distance(fingerPos, this.transform.position);
        return distance <= this.interactingSize;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.interactingSize, this.interactingSize, 0));
    }
#endif
}