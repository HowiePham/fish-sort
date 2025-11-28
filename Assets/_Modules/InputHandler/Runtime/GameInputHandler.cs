using Lean.Touch;
using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    [SerializeField] private float swappingThreshold;
    [SerializeField] private Fish selectedFish;
    private Vector3 fingerOffset;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDownHandler;
        LeanTouch.OnFingerUp += FingerUpHandler;
        LeanTouch.OnFingerUpdate += FingerUpdateHandler;
    }

    private void FingerUpdateHandler(LeanFinger finger)
    {
    }

    private void FingerDownHandler(LeanFinger finger)
    {
        Vector3 fingerPos = finger.GetWorldPosition(10);
    }

    private void FingerUpHandler(LeanFinger finger)
    {
        this.selectedFish = null;
        this.fingerOffset = Vector3.zero;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDownHandler;
        LeanTouch.OnFingerUp -= FingerUpHandler;
        LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
    }
}