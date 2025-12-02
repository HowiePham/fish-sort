using Cysharp.Threading.Tasks;
using Lean.Touch;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishVisual fishVisual;
    [SerializeField] private FishInteracting fishInteracting;
    [SerializeField] private FishType fishType;
    public Bounds Bounds => this.fishVisual.GetBounds();
    public int FishTypeNumber => this.fishType.TypeNumber;

    public bool CanSelectFish(LeanFinger finger)
    {
        return this.fishInteracting.CanSelectFish(finger);
    }

    public async UniTask MoveTo(Vector3 entryPos, FishHolder fishHolder)
    {
        await this.fishInteracting.MoveTo(entryPos, fishHolder);
    }

    public async UniTask JumpTo(Vector3 waterPos, FishHolder fishHolder)
    {
        await this.fishInteracting.JumpTo(waterPos, fishHolder);
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