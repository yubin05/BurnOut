using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, ICollision2D
{
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            characterObject.MotionHandler.EndJump();

            if (characterObject is PlayerObject)
            {
                var playerObj = characterObject as PlayerObject;
                var player = playerObj.data as Player;
                player.IsDoubleJump = false;
            }

            // 착지하는 순간, 미끄러짐 등을 방지하기 위해 순간적으로 속력 한번 초기화
            characterObject.Rigidbody2D.velocity = Vector3.zero;
        }
    }

    public virtual void OnCollisionExit2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            // 땅에서 벗어나는 경우 점프하지 않아도 점프한 것으로 판정
            characterObject.MotionHandler.StartJump();
        }
    }
}
