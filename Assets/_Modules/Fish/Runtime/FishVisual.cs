using UnityEngine;

public class FishVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject outline;
    
    public void SetFishVisual(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
    }

    public void SetActiveOutline(bool active)
    {
        this.outline.SetActive(active);
    }

    public Bounds GetBounds()
    {
        return this.spriteRenderer.bounds;
    }
}