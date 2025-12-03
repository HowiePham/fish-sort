using Mimi.VisualActions;
using UnityEngine;

public class EnemyDeadCondition : VisualCondition
{
    [SerializeField] private bool isDead;

    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
    }

    public override bool Validate()
    {
        return this.isDead;
    }
}