using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Mimi.VisualActions;
using Mimi.VisualActions.Attribute;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VisualActions.VisualTransform
{
    [TypeInfoBox("Move Transform into target transform by Dotween")]
    public class MoveTransformByTween : VisualAction
    {
        [MainInput] [SerializeField] private Transform movingTransform;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private bool isWaitToComplete;

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                if (isWaitToComplete)
                {
                    TweenerCore<Vector3, Vector3, VectorOptions> tween = this.movingTransform.DOMove(this.targetTransform.position, this.duration).SetEase(this.ease);
                    cancellationToken.Register(() => tween.Kill());
                    await tween.AsyncWaitForCompletion();
                }
                else
                {
                    movingTransform.DOMove(targetTransform.position, duration);
                }
            }
            catch (OperationCanceledException e)
            {
            }
        }
    }
}