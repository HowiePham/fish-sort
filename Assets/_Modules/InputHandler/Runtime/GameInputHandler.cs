using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    [Header("Input Config")] [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField] private float speedScalar = 2f;
    [SerializeField] private Vector3 draggingOffset;
    private Vector3 dragVelocity;

    [Header("Game Interacting")] [SerializeField]
    private Fish selectedFish;

    [SerializeField] private bool isDragging;
    [SerializeField] private List<Fish> fishList;
    [SerializeField] private FishTank[] fishTanks;
    private Vector3 fingerOffset;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDownHandler;
        LeanTouch.OnFingerUp += FingerUpHandler;
        LeanTouch.OnFingerOld += FingerOldHandler;
        LeanTouch.OnFingerUpdate += FingerUpdateHandler;
    }

    private void FingerOldHandler(LeanFinger finger)
    {
        if (this.selectedFish == null)
        {
            this.isDragging = false;
            return;
        }

        this.isDragging = true;
    }

    private void FingerUpdateHandler(LeanFinger finger)
    {
        if (!this.isDragging)
        {
            return;
        }

        Debug.Log($"--- (INPUT) Dragging Fish");
        Vector3 fingerPos = finger.GetWorldPosition(10);
        Vector3 newPos = fingerPos + this.draggingOffset;

        Transform selectedFishTransform = this.selectedFish.transform;
        selectedFishTransform.position = Vector3.SmoothDamp(selectedFishTransform.position, newPos, ref this.dragVelocity, this.smoothTime,
            Mathf.Infinity, Time.deltaTime * this.speedScalar);
    }

    private void FingerDownHandler(LeanFinger finger)
    {
        if (CanSelectAnyFish(finger, out Fish selectedFish))
        {
            Debug.Log($"--- (INPUT) Select Fish");
            this.selectedFish = selectedFish;
            return;
        }

        if (this.selectedFish == null)
        {
            return;
        }

        Debug.Log($"--- (INPUT) Select Tank");
        FishTank fishTank = SelectFishTank(finger, out FishHolder fishHolder);

        if (fishTank == null)
        {
            this.selectedFish.MoveBack();
        }
        else
        {
            this.selectedFish.JumpTo(fishTank.WaterPos, fishHolder);
        }

        this.selectedFish = null;
    }

    private void FingerUpHandler(LeanFinger finger)
    {
        if (this.selectedFish == null)
        {
            this.isDragging = false;
            return;
        }

        if (this.isDragging)
        {
            Debug.Log($"--- (INPUT) End dragging fish");
            FishTank fishTank = SelectFishTank(finger, out FishHolder fishHolder);

            if (fishTank == null)
            {
                this.selectedFish.MoveBack();
            }
            else
            {
                this.selectedFish.MoveTo(fishTank.EntryPos, fishHolder);
            }

            this.selectedFish = null;
            this.isDragging = false;
        }
    }

    private bool CanSelectAnyFish(LeanFinger finger, out Fish selectedFish)
    {
        foreach (Fish fish in this.fishList)
        {
            if (fish.CanSelectFish(finger))
            {
                selectedFish = fish;
                return true;
            }
        }

        selectedFish = null;
        return false;
    }

    private FishTank SelectFishTank(LeanFinger finger, out FishHolder fishHolder)
    {
        foreach (FishTank fishTank in this.fishTanks)
        {
            FishHolder holder = fishTank.SelectFishHolder(finger);
            if (holder == null)
            {
                continue;
            }

            fishHolder = holder;
            return fishTank;
        }

        fishHolder = null;
        return null;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDownHandler;
        LeanTouch.OnFingerUp -= FingerUpHandler;
        LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
        LeanTouch.OnFingerOld -= FingerOldHandler;
    }
}