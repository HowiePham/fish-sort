using Lean.Touch;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishVisual fishVisual;
    [SerializeField] private FishInteracting fishInteracting;
    [SerializeField] private FishType fishType;
    public Bounds Bounds => this.fishVisual.GetBounds();

    public bool CanSelectFish(LeanFinger finger)
    {
        return this.fishInteracting.CanSelectFish(finger);
    }

    public void MoveTo(Vector3 entryPos, FishHolder fishHolder)
    {
        this.fishInteracting.MoveTo(entryPos, fishHolder);
    }

    public void JumpTo(Vector3 waterPos, FishHolder fishHolder)
    {
        this.fishInteracting.JumpTo(waterPos, fishHolder);
    }

    public void MoveBack()
    {
        this.fishInteracting.MoveBack();
    }

    public void SetFishVisual(Sprite sprite)
    {
        this.fishVisual.SetFishVisual(sprite);
    }
}