using Lean.Touch;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    [SerializeField] private Transform aimingLine;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDownHandler;
    }

    private void FingerDownHandler(LeanFinger finger)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.aimingLine.position, this.aimingLine.up, Mathf.Infinity);

        Collider2D hitCollider = hit.collider;
        if (hitCollider == null)
        {
            return;
        }

        Debug.Log($"Shoot at {hitCollider.name}");
        var enemy = hitCollider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Kill();
        }
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDownHandler;
    }
}