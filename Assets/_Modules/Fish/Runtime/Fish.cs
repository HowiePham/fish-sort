using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishVisual fishVisual;
    [SerializeField] private FishInteracting fishInteracting;
    [SerializeField] private FishType fishType;
    public Bounds Bounds => this.fishVisual.GetBounds();
    

    public void SetFishVisual(Sprite sprite)
    {
        this.fishVisual.SetFishVisual(sprite);
    }
}