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
    private Vector3 dragVelocity;

    [Header("Fish Moving Config")] private FishMoving fishMoving;
    [SerializeField] private Vector3 oldDestination;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float movingDuration;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float sinkDepth = 0.5f;
    [SerializeField] private float sinkDuration = 0.3f;
    [SerializeField] private float swimUpDuration = 0.5f;
    [SerializeField] private FishTank currentFishTank;
    [SerializeField] private FishTank selectedFishTank;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDownHandler;
        LeanTouch.OnFingerUp += FingerUpHandler;
        LeanTouch.OnFingerUpdate += FingerUpdateHandler;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        this.fishMoving = new FishMoving();
    }

    private void FingerUpdateHandler(LeanFinger finger)
    {
        if (!this.isSelected)
        {
            return;
        }

        Vector3 fingerPos = finger.GetWorldPosition(10);
        Vector3 newPos = fingerPos + this.draggingOffset;

        this.transform.position = Vector3.SmoothDamp(this.transform.position, newPos, ref this.dragVelocity, this.smoothTime,
            Mathf.Infinity, Time.deltaTime * this.speedScalar);
    }

    private void FingerDownHandler(LeanFinger finger)
    {
        if (CanSelectFish(finger))
        {
            this.isSelected = true;
        }
    }

    private void FingerUpHandler(LeanFinger finger)
    {
        if (!this.isSelected)
        {
            return;
        }

        this.isSelected = false;
        this.isMoving = true;
        MoveFish();
    }

    private async UniTask MoveFish()
    {
        if (this.selectedFishTank == null)
        {
            await this.fishMoving.MoveToPosition(this.transform, this.oldDestination, this.movingDuration);
        }
        else
        {
            Vector3 entryPos = this.selectedFishTank.EntryPos;
            Vector3 holderPos = this.selectedFishTank.OccupyEmptyHolder(1);
            this.transform.position = this.currentFishTank.EntryPos;

            await UniTask.WaitForSeconds(0.5f);

            await this.fishMoving.JumpToPosition(this.transform, entryPos, holderPos, this.jumpHeight, this.sinkDepth,
                this.jumpDuration, this.sinkDuration, this.swimUpDuration);

            this.oldDestination = holderPos;
            this.currentFishTank = this.selectedFishTank;
        }

        this.selectedFishTank = null;
        this.isMoving = false;
    }

    private bool CanSelectFish(LeanFinger finger)
    {
        if (this.isMoving)
        {
            return false;
        }

        Vector3 fingerPos = finger.GetWorldPosition(10);
        return this.boxCollider.bounds.Contains(fingerPos);
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDownHandler;
        LeanTouch.OnFingerUp -= FingerUpHandler;
        LeanTouch.OnFingerUpdate -= FingerUpdateHandler;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var fishTank = collider.GetComponent<FishTank>();
        if (fishTank == null || fishTank.IsFull() || fishTank == this.currentFishTank)
        {
            this.selectedFishTank = null;
            return;
        }

        this.selectedFishTank = fishTank;
    }
}