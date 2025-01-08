using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IGrid
{
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            if (characterObject is EnemyObject)
            {
                var enemyObject = characterObject as EnemyObject;
                var enemy = enemyObject.data as Enemy;

                if (enemy.MoveType == 0)    // MoveType이 0이면 벽에 닿을 경우, 방향 전환
                {
                    enemyObject.MoveDirection *= -1;
                }
            }
        }
    }

    public virtual void OnCollisionExit2D(Collision2D other)
    {
    }
}
