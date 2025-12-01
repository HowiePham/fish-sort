using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FishMoving
{
    private Sequence movingSequence;

    public async UniTask JumpToPosition(Transform targetObject, Vector3 entryPos, Vector3 destination, float jumpHeight,
        float sinkDepth, float jumpDuration, float sinkDuration, float swimUpDuration)
    {
        this.movingSequence?.Kill();

        var sinkPos = new Vector3(entryPos.x, entryPos.y - sinkDepth, entryPos.z);

        this.movingSequence = DOTween.Sequence();

        this.movingSequence.Append(
            targetObject.DOJump(entryPos, jumpHeight, 1, jumpDuration)
                .SetEase(Ease.InOutQuad)
        );

        this.movingSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );

        this.movingSequence.Append(
            targetObject.DOMove(destination, swimUpDuration)
                .SetEase(Ease.OutQuad)
        );

        await this.movingSequence.Play().AsyncWaitForCompletion();
    }

    public async UniTask SinkToPosition(Transform targetObject, Vector3 entryPos, Vector3 destination,
        float sinkDepth, float moveDuration, float sinkDuration, float swimUpDuration)
    {
        this.movingSequence?.Kill();

        var waterPos = new Vector3(entryPos.x, entryPos.y - 3.25f, entryPos.z);
        var sinkPos = new Vector3(waterPos.x, waterPos.y - sinkDepth, waterPos.z);

        this.movingSequence = DOTween.Sequence();

        this.movingSequence.Append(
            targetObject.DOMove(waterPos, moveDuration)
                .SetEase(Ease.InOutQuad)
        );

        this.movingSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );

        this.movingSequence.Append(
            targetObject.DOMove(destination, swimUpDuration)
                .SetEase(Ease.OutQuad)
        );

        await this.movingSequence.Play().AsyncWaitForCompletion();
    }

    public async UniTask MoveToPosition(Transform targetObject, Vector3 destination, float duration)
    {
        await targetObject.DOMove(destination, duration).AsyncWaitForCompletion();
    }
}