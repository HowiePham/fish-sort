using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class FishMoving
{
    private Sequence movingSequence;

    public async UniTask JumpToPosition(Transform targetObject, Vector3 waterPos, Vector3 destination, float jumpHeight,
        float sinkDepth, float jumpDuration, float sinkDuration, float swimUpDuration)
    {
        this.movingSequence?.Kill();

        var startPos = targetObject.position;
        var sinkPos = new Vector3(waterPos.x, waterPos.y - sinkDepth, waterPos.z);

        float heightDiff = startPos.y - waterPos.y;
        float adjustedJumpHeight = heightDiff > 0 ? heightDiff + jumpHeight : jumpHeight;

        var peakPos = new Vector3(
            (startPos.x + waterPos.x) / 2,
            startPos.y + adjustedJumpHeight,
            (startPos.z + waterPos.z) / 2
        );

        float jumpUpDuration = jumpDuration * 0.5f;
        float fallDownDuration = jumpDuration * 0.5f;

        this.movingSequence = DOTween.Sequence();

        var totalPoints = 40;
        var peakIndex = 8;

        Vector3[] fullPath = new Vector3[totalPoints];

        for (var i = 0; i < totalPoints; i++)
        {
            if (i <= peakIndex)
            {
                float t = i / (float)peakIndex;
                fullPath[i].x = Mathf.Lerp(startPos.x, peakPos.x, t);
                fullPath[i].z = Mathf.Lerp(startPos.z, peakPos.z, t);
                float easedT = t * t * (3f - 2f * t);
                fullPath[i].y = startPos.y + adjustedJumpHeight * easedT;
            }
            else
            {
                float t = (i - peakIndex) / (float)(totalPoints - 1 - peakIndex);
                fullPath[i].x = Mathf.Lerp(peakPos.x, waterPos.x, t);
                fullPath[i].z = Mathf.Lerp(peakPos.z, waterPos.z, t);
                fullPath[i].y = peakPos.y - adjustedJumpHeight * (t * t);
            }
        }

        this.movingSequence.Append(
            targetObject.DOPath(fullPath, jumpDuration, PathType.CatmullRom)
                .SetEase(Ease.Linear)
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