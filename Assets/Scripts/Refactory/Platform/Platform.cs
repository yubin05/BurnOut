using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IGrid
{
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            var rigid = characterObject.GetComponent<Rigidbody2D>();
            // 땅에 착지했을 때만 점프가 끝났다고 판정
            // 발판 옆에 닿은 경우는 점프가 끝난 것이 아님
            if (rigid.velocity == Vector2.zero) 
            {
                characterObject.MotionHandler.IsJump = false;
            }
        }
    }

    public virtual void OnCollisionExit2D(Collision2D other)
    {
        var characterObject = other.transform.GetComponent<CharacterObject>();
        if (characterObject != null)
        {
            // 땅에서 벗어나는 경우 점프할 수 없는 상태로 변경
            characterObject.MotionHandler.IsJump = true;
        }
    }
}
