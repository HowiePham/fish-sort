using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyDeadCondition enemyDeadCondition;

    public void Kill()
    {
        this.enemyDeadCondition.SetDead(true);
    }
}