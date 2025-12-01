using Cysharp.Threading.Tasks;
using Lean.Touch;
using UnityEngine;

public class FishInteracting : MonoBehaviour
{
    [Header("Draggable Config")] [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float speedScalar = 2f;
    [SerializeField] private Vector3 draggingOffset;
    [SerializeField] private bool isSelected;
    [SerializeField] private bool isMoving;

    [Header("Fish Moving Config")] private FishMoving fishMoving;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float movingDuration;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float sinkDepth = 0.5f;
    [SerializeField] private float sinkDuration = 0.3f;
    [SerializeField] private float swimUpDuration = 0.5f;
    [SerializeField] private FishHolder currentFishHolder;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        this.fishMoving = new FishMoving();
    }

    public async UniTask MoveTo(Vector3 entryPos, FishHolder fishHolder)
    {
        this.isMoving = true;

        Vector3 holderPos = fishHolder.Position;
        this.transform.position = entryPos;
        this.currentFishHolder.LeaveHolder();

        await this.fishMoving.SinkToPosition(this.transform, entryPos, holderPos, this.sinkDepth,
            this.movingDuration, this.sinkDuration, this.swimUpDuration);

        this.currentFishHolder = fishHolder;
        this.isMoving = false;
    }

    public async UniTask JumpTo(Vector3 waterPos, FishHolder fishHolder)
    {
        this.isMoving = true;

        Vector3 holderPos = fishHolder.Position;
        this.currentFishHolder.LeaveHolder();

        await this.fishMoving.JumpToPosition(this.transform, waterPos, holderPos, this.jumpHeight, this.sinkDepth,
            this.jumpDuration, this.sinkDuration, this.swimUpDuration);

        this.currentFishHolder = fishHolder;
        this.isMoving = false;
    }

    public async UniTask MoveBack()
    {
        await this.fishMoving.MoveToPosition(this.transform, this.currentFishHolder.Position, this.movingDuration);
        this.isMoving = false;
    }

    public bool CanSelectFish(LeanFinger finger)
    {
        if (this.isMoving)
        {
            return false;
        }

        Vector3 fingerPos = finger.GetWorldPosition(10);
        return this.boxCollider.bounds.Contains(fingerPos);
    }
}