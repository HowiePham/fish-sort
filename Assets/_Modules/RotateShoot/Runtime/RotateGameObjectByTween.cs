using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mimi.VisualActions;
using Mimi.VisualActions.Attribute;
using UnityEngine;

public class RotateGameObjectByTween : VisualAction
{
    [SerializeField] [MainInput] private Transform targetTransform;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float duration = 0.3f;
    
    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        try
        {
            var tween = this.targetTransform.DORotate(this.targetRotation, this.duration).SetEase(Ease.Linear);
            cancellationToken.Register(() => tween.Kill());
            await tween.AsyncWaitForCompletion();
        }
        catch (OperationCanceledException e)
        {
        }
    }
}