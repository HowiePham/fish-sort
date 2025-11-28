using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mimi.VisualActions;
using UnityEngine;

public class RotateByTween : VisualAction
{
    [SerializeField] private Transform rotateObject;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 rotation;

    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        await this.rotateObject.DORotate(this.rotation, this.duration).AsyncWaitForCompletion();
    }
}