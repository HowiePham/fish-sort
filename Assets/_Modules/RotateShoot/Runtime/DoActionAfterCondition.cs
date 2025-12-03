using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mimi.VisualActions;
using UnityEngine;
using UnityEngine.Events;

public class DoActionAfterCondition : VisualAction
{
    [SerializeField] private VisualCondition condition;
    public UnityEvent Action;

    protected override async UniTask OnExecuting(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.WaitUntil(() => this.condition.Validate());
            this.Action?.Invoke();
        }
        catch (OperationCanceledException e)
        {
        }
    }
}