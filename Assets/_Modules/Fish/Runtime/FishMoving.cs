using System.Collections;
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

        Quaternion oldRotation = targetObject.rotation;
        Vector3 startPos = targetObject.position;
        var sinkPos = new Vector3(waterPos.x, waterPos.y - sinkDepth, waterPos.z);

        float heightDiff = startPos.y - waterPos.y;

        float adjustedJumpHeight = jumpHeight;
        if (heightDiff > 0)
        {
            adjustedJumpHeight = heightDiff + jumpHeight;
        }

        await JumpCoroutine(targetObject, jumpDuration, adjustedJumpHeight, startPos, waterPos, sinkPos);

        this.movingSequence = DOTween.Sequence();

        this.movingSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );
        this.movingSequence.Join(
            targetObject.DORotateQuaternion(oldRotation, sinkDuration));

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
        Quaternion oldRotation = targetObject.rotation;

        var waterPos = new Vector3(entryPos.x, entryPos.y - 3.25f, entryPos.z);
        var sinkPos = new Vector3(waterPos.x, waterPos.y - sinkDepth, waterPos.z);

        Vector3 directionToWater = (waterPos - entryPos).normalized;
        float angleToWater = Mathf.Atan2(directionToWater.y, directionToWater.x) * Mathf.Rad2Deg;
        Quaternion rotationToWater = Quaternion.Euler(0, 0, angleToWater);
        targetObject.rotation = rotationToWater;

        this.movingSequence = DOTween.Sequence();

        this.movingSequence.Append(
            targetObject.DOMove(waterPos, moveDuration)
                .SetEase(Ease.InOutQuad)
        );

        this.movingSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );
        this.movingSequence.Join(
            targetObject.DORotateQuaternion(oldRotation, sinkDuration));

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

    private async UniTask JumpCoroutine(Transform transform, float jumpDuration, float jumpHeight, Vector2 start, Vector2 end, Vector2 sinkPos)
    {
        float elapsed = 0f;

        Vector2 midPoint = (start + end) / 2f;
        Vector2 controlPoint = midPoint + Vector2.up * jumpHeight;

        Vector2 prevPosition = start;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float linearT = elapsed / jumpDuration;

            // Áp dụng easing curve mới - cá ở đỉnh lâu hơn
            float t = ApplyJumpEasing(linearT);

            Vector2 position = CalculateQuadraticBezier2D(start, controlPoint, end, t);
            transform.position = new Vector3(position.x, position.y, transform.position.z);

            // Xoay cá theo hướng di chuyển
            Vector2 direction = (position - prevPosition).normalized;
            if (direction != Vector2.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            prevPosition = position;

            await UniTask.WaitForEndOfFrame();
        }

        // Đảm bảo cá đến đúng điểm cuối
        Vector2 finalPos = end;
        transform.position = new Vector3(finalPos.x, finalPos.y, transform.position.z);
        Vector2 directionToNext = (sinkPos - finalPos).normalized;
        if (directionToNext != Vector2.zero)
        {
            float smoothAngle = Mathf.Atan2(directionToNext.y, directionToNext.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
    }

    /// <summary>
    /// Easing function để cá ở đỉnh lâu hơn:
    /// - Nhanh ở đầu (lên nhanh)
    /// - Chậm ở giữa/đỉnh (ở đỉnh lâu hơn)
    /// - Nhanh ở cuối (rơi nhanh)
    /// </summary>
    private float ApplyJumpEasing(float t)
    {
        // PHƯƠNG PHÁP 1: Custom easing với "plateau" ở giữa (KHUYÊN DÙNG)
        // Tạo vùng chậm rõ rệt ở khoảng t = 0.4 đến 0.6
        if (t < 0.5f)
        {
            // Nửa đầu: tăng tốc rồi chậm lại khi gần đỉnh
            float localT = t / 0.5f; // Chuẩn hóa về [0,1]
            return 0.5f * Mathf.Pow(localT, 1.5f); // Tăng nhanh đầu, chậm dần
        }
        else
        {
            // Nửa sau: chậm khi rời đỉnh, tăng tốc khi rơi xuống
            float localT = (t - 0.5f) / 0.5f; // Chuẩn hóa về [0,1]
            return 0.5f + 0.5f * Mathf.Pow(localT, 0.67f); // Chậm đầu, nhanh dần
        }

        // PHƯƠNG PHÁP 2: Sine-based với điều chỉnh (tùy chọn)
        // Uncomment dòng dưới nếu muốn dùng
        // return t - 0.15f * Mathf.Sin(t * Mathf.PI * 2f) / (Mathf.PI * 2f);

        // PHƯƠNG PHÁP 3: Polynomial với vùng phẳng ở giữa (tùy chọn)
        // Uncomment dòng dưới nếu muốn dùng
        // return Mathf.Pow(t, 2f) * (3f - 2f * t) * (1f - 0.3f * Mathf.Sin(t * Mathf.PI));
    }

    /// <summary>
    /// Tính toán điểm trên đường cong Bezier bậc 2 trong 2D (Quadratic Bezier)
    /// </summary>
    private Vector2 CalculateQuadraticBezier2D(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = uu * p0;
        point += 2 * u * t * p1;
        point += tt * p2;

        return point;
    }
}