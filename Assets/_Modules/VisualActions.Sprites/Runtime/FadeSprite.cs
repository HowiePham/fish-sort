using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mimi.VisualActions.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mimi.VisualActions.Sprites
{
    public class FadeSprite : VisualAction
    {
        [MainInput]
        [SerializeField, Required] private SpriteRenderer target;
        [SerializeField, Range(0f, 1f)] private float startAlpha;
        [SerializeField, Range(0f, 1f)] private float endAlpha;
        [SerializeField] private Ease ease;
        [SerializeField, MinValue(0f)] private float duration = 1f;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            Color color = this.target.color;
            color.a = this.startAlpha;
            this.target.color = color;
            await this.target.DOFade(this.endAlpha, this.duration).SetEase(this.ease).AsyncWaitForCompletion();
        }
    }
}