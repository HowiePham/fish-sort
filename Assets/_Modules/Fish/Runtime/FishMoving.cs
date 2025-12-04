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

        // Sửa lại: truyền đúng tham số vào JumpCoroutine
        await JumpCoroutine(targetObject, jumpDuration, adjustedJumpHeight, startPos, waterPos);

        this.movingSequence = DOTween.Sequence();

        // Chìm xuống sinkPos và xoay cá theo hướng chìm
        this.movingSequence.Append(
            targetObject.DOMove(sinkPos, sinkDuration)
                .SetEase(Ease.OutQuad)
        );
        // this.movingSequence.Join(
        //     targetObject.DORotateQuaternion(oldRotation, sinkDuration)
        //         .SetEase(Ease.InQuad)
        // );

        // Bơi về destination với xoay mượt về hướng đích
        float rotationDuration = Mathf.Min(swimUpDuration * 0.4f, 0.5f); // Xoay trong 40% thời gian đầu hoặc tối đa 0.5s

        this.movingSequence.Append(
            targetObject.DOMove(destination, swimUpDuration)
                .SetEase(Ease.OutQuad)
        );
        // this.movingSequence.Join(
        //     targetObject.DORotate(oldRotation.eulerAngles, rotationDuration)
        //         .SetEase(Ease.OutQuad)
        // );

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

    private async UniTask JumpCoroutine(Transform transform, float jumpDuration, float jumpHeight, Vector2 start, Vector2 end)
    {
        float elapsed = 0f;

        // Tính điểm kiểm soát (control point) cho Bezier curve
        // Điểm này nằm ở giữa và cao hơn (trên trục Y) để tạo quỹ đạo nhảy
        Vector2 midPoint = (start + end) / 2f;
        Vector2 controlPoint = midPoint + Vector2.up * jumpHeight;

        Vector2 prevPosition = start;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float linearT = elapsed / jumpDuration;

            // Áp dụng easing curve cho chuyển động
            // Nhanh lên đỉnh, chậm ở đỉnh, rơi nhanh xuống
            float t = ApplyJumpEasing(linearT);

            // Tính vị trí trên đường cong Bezier bậc 2 (2D)
            Vector2 position = CalculateQuadraticBezier2D(start, controlPoint, end, t);
            transform.position = new Vector3(position.x, position.y, transform.position.z);

            // Xoay cá theo hướng di chuyển (chỉ xoay trục Z trong 2D)
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
    }

    /// <summary>
    /// Easing function mô phỏng chuyển động thực tế mượt mà:
    /// - Nhanh ở đầu (tăng tốc)
    /// - Chậm nhất ở đỉnh (nhưng vẫn có chuyển động)
    /// - Nhanh dần khi rơi xuống (tăng tốc do trọng lực)
    /// </summary>
    private float ApplyJumpEasing(float t)
    {
        // Sử dụng hàm sine để tạo chuyển động mượt mà liên tục
        // Đường cong này không bao giờ có đạo hàm = 0 (không dừng hẳn)

        // Phương pháp 1: Sine-based easing (mượt nhất)
        // return t - Mathf.Sin(t * Mathf.PI * 2f) / (Mathf.PI * 2f);

        // Phương pháp 2: Smoothstep với điều chỉnh (cân bằng giữa mượt và tự nhiên)
        // Công thức: 3t² - 2t³ (Smoothstep chuẩn)
        return t * t * (3f - 2f * t);

        // Phương pháp 3: Custom curve với tốc độ không đổi ở đỉnh
        // float adjustedT = Mathf.Pow(t, 1.5f);
        // return adjustedT;
    }

    /// <summary>
    /// Tính toán điểm trên đường cong Bezier bậc 2 trong 2D (Quadratic Bezier)
    /// </summary>
    private Vector2 CalculateQuadraticBezier2D(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        // Công thức: B(t) = (1-t)²P0 + 2(1-t)tP1 + t²P2
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = uu * p0; // (1-t)² * P0
        point += 2 * u * t * p1; // 2(1-t)t * P1
        point += tt * p2; // t² * P2

        return point;
    }
}