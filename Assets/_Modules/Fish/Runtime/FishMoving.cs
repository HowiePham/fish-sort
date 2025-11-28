using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FishMoving
{
    public async UniTask JumpToPosition(Transform targetObject, Vector3 destination, float jumpPower, float duration)
    {
        await targetObject.DOJump(destination, jumpPower, 1, duration).AsyncWaitForCompletion();
    }

    public async UniTask MoveToPosition(Transform targetObject, Vector3 destination, float duration)
    {
        await targetObject.DOMove(destination, duration).AsyncWaitForCompletion();
    }
}