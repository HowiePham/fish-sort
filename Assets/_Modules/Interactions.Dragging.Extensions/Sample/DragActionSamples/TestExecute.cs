using System.Threading;
using Cysharp.Threading.Tasks;
using Mimi.VisualActions;
using UnityEngine;

namespace Drag.Sample
{
    public class TestExecute : MonoBehaviour
    {
        [SerializeField] private VisualAction action;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async UniTask Start()
        {
            await action.Initialize();
            await action.Execute(CancellationToken.None);
        }

        
    }
}
