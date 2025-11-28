using UnityEngine;

public class FishVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetFishVisual(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
    }

    public Bounds GetBounds()
    {
        return this.spriteRenderer.bounds;
    }
}