using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public EnemyFSM enemyFSM;

    public void PlayerHit()
    {
        enemyFSM.AttackAction();
    }
}
