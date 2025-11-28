using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Serialization;

public class FishInteracting : MonoBehaviour
{
    [Header("Draggable Config")] [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float speedScalar = 2f;
    [SerializeField] private Vector3 draggingOffset;
    [SerializeField] private bool isSelected;
    private Vector3 fingerOffset;
    private Vector3 dragVelocity;

    [Header("Fish Moving Config")] private FishMoving fishMoving;
    [SerializeField] private Vector3 oldDestination;
    [SerializeField] private Transform destination;
    [SerializeField] private Ease fallEase = Ease.InQuad;
    [SerializeField] private float throwPower;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float movingDuration;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDownHandler;
        LeanTouch.OnFingerUp += FingerUpHandler;
        LeanTouch.OnFingerUpdate += FingerUpdateHandler;
    }

    private void FingerUpdateHandler(LeanFinger finger)
    {
        if (this.isSelected)
        {
            Vector3 fingerPos = finger.GetWorldPosition(10);
            Vector3 destination = fingerPos + this.fingerOffset;
            // this.transform.position = fingerPos + this.draggingOffset;

            this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref this.dragVelocity, this.smoothTime,
                Mathf.Infinity, Time.deltaTime * this.speedScalar);
        }
    }

    private void FingerDownHandler(LeanFinger finger)
    {
        if (CanSelectFish(finger))
        {
            this.isSelected = true;
        }
    }

    private void FingerUpHandler(LeanFinger finger)
    {
        if (!this.isSelected)
        {
            return;
        }

        MoveFish();
    }

    private async UniTask MoveFish()
    {
        if (this.destination == null)
        {
            await this.fishMoving.MoveToPosition(this.transform, this.oldDestination, this.movingDuration);
        }
        else
        {
            this.transform.position = this.oldDestination;
            Vector3 destinationPos = this.destination.position;
            await this.fishMoving.JumpToPosition(this.transform, destinationPos, this.throwPower, this.jumpDuration);
            this.oldDestination = destinationPos;
        }

        this.isSelected = false;
        this.fingerOffset = Vector3.zero;
        this.destination = null;
    }

    private bool CanSelectFish(LeanFinger finger)
    {
        Vector3 fingerPos = finger.GetWorldPosition(10);
        return this.boxCollider.bounds.Contains(fingerPos);
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDownHandler;
        LeanTouch.OnFingerUp -= FingerUpHandler;
        LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log($"Fish In Holder");
        var fishHolder = collider.GetComponent<FishHolder>();
        if (fishHolder == null)
        {
            return;
        }

        this.destination = collider.transform;
    }
}