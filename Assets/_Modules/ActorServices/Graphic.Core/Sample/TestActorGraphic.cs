using Mimi.Actor.Graphic.Core;
using Mimi.Actor.Graphic.SpriteRenderer;
using Mimi.VisualActions.Attribute;
using Mimi.VisualActions.Attribute.Editor;
using Mimi.VisualActions.Sprites.Editor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class TestActorGraphic : MonoBehaviour
{
    [SerializeField] private BaseMonoGraphic targetGraphic;
    [SerializeField] private MonoGraphicAsset graphicSpriteAsset;
    [SerializeField] private int sortingOrder;
    [SerializeField,SortingLayerPopup] private string sortingLayerName;

    [Button]
    public void SetAssetGraphic()
    {
        targetGraphic.SetAssetGraphic(graphicSpriteAsset);
       
    }

    [Button]
    public void SetSortingOrder()
    {
        targetGraphic.SetSortingOrder(sortingOrder);
        
    }

    [Button]
    public void SetSortingLayerName()
    {
        targetGraphic.SetSortingLayerName(sortingLayerName);
    }
}
