using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mimi.VisualActions.ControlFlow
{
    public class VisualLoop : VisualAction
    {
        [SerializeField] private VisualCondition conditionToFinish;
        [SerializeField] private VisualAction action;

        private CancellationTokenSource cancellationTokenSource;
        protected override async  UniTask OnInitializing()
        {
            await base.OnInitializing();
            await action.Initialize();
        }

        protected override async UniTask OnExecuting(CancellationToken cancellationToken)
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                while (!conditionToFinish.Validate())
                {
                    await action.Execute(cancellationTokenSource.Token);
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            cancellationTokenSource?.Cancel();
        }

        [ContextMenu("Cancel")]
        public override void Cancel()
        {
            cancellationTokenSource?.Cancel();
        }
        
    }
}