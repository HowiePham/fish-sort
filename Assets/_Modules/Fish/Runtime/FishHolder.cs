using System;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class FishHolder : MonoBehaviour
{
    [SerializeField] private Transform fishInHolderTransform;
    [SerializeField] private Vector3 movingEffectDirection;
    [SerializeField] private float movingEffectDuration;
    [SerializeField] private int fishTypeNumber;
    [SerializeField] private float interactingSize;
    [SerializeField] private bool isOccupied;
    private Sequence finishSequence;

    public Vector3 Position => this.transform.position;

    public Action OnFishMovingIn;
    public bool IsOccupied => this.isOccupied;
    public int FishTypeNumber => this.fishTypeNumber;

    public void FinishHolder()
    {
        this.finishSequence.Append(
            fishInHolderTransform.DOMove(this.movingEffectDirection, this.movingEffectDuration)
                .SetEase(Ease.OutQuad)
        );

        // this.finishSequence.Append(
        //     fishInHolderTransform.DOMove(destination, this.movingEffectDuration)
        //         .SetEase(Ease.OutQuad)
        // );
    }

    public void OccupyHolder(int typeNumber, Transform fishTransform)
    {
        this.isOccupied = true;
        this.fishTypeNumber = typeNumber;
        this.fishInHolderTransform = fishTransform;
        fishTransform.SetParent(this.transform);
        OnFishMovingIn?.Invoke();
    }

    public void LeaveHolder()
    {
        this.fishTypeNumber = -1;
        this.isOccupied = false;
        this.fishInHolderTransform = null;
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