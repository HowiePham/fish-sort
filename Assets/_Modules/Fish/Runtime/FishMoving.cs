using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FishMoving
{
    private Sequence jumpSequence;

    public async UniTask JumpToPosition(Transform targetObject, Vector3 entryPos, Vector3 destination, float jumpHeight,
        float sinkDepth, float jumpDuration, float sinkDuration, float swimUpDuration)
    {
        this.jumpSequence?.Kill();

        var sinkPos = new Vector3(entryPos.x, entryPos.y - sinkDepth, entryPos.z);

        this.jumpSequence = DOTween.Sequence();

        this.jumpSequence.Append(
            targetObject.DOJump(entryPos, jumpHeight, 1, jumpDuration)
                .SetEase(Ease.InOutQuad)
        );

        this.jumpSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );

        this.jumpSequence.Append(
            targetObject.DOMove(destination, swimUpDuration)
                .SetEase(Ease.OutQuad)
        );

        await this.jumpSequence.Play().AsyncWaitForCompletion();
    }

    public async UniTask MoveToPosition(Transform targetObject, Vector3 destination, float duration)
    {
        await targetObject.DOMove(destination, duration).AsyncWaitForCompletion();
    }
}