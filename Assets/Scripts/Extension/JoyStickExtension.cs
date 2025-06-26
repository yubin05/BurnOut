using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class JoyStickExtension
{
    /// <summary>
    /// 조이스틱 원 바깥으로 나가지 않도록 위치 값 계산해주는 함수
    /// </summary>
    /// <param name="joystickBgRect">조이스틱 배경 이미지 렉트 트랜스폼</param>
    /// <param name="touchPos">사용자가 터치한 위치</param>
    /// <returns>최종적으로 계산된 조이스틱 위치</returns>
    private static Vector2 CalculatePos(RectTransform joystickBgRect, Vector2 touchPos)
    {
        // 원의 중심
        Vector2 center = new Vector2(
            joystickBgRect.anchoredPosition.x + joystickBgRect.rect.width / 2,
            joystickBgRect.anchoredPosition.y + joystickBgRect.rect.height / 2
        );
        // 반지름
        float radius = joystickBgRect.rect.width / 2;

        var distance = Vector2.Distance(touchPos, center);    // 조이스틱 중심으로부터 터치 위치까지 거리
        // 조이스틱이 원 밖으로 나가는 경우 조이스틱 나가지 않도록 예외 처리
        if (distance > radius)
        {
            touchPos.x = center.x + (touchPos.x - center.x) * radius / distance;
            touchPos.y = center.y + (touchPos.y - center.y) * radius / distance;
        }

        return new Vector2(touchPos.x, touchPos.y);
    }

    /// <summary>
    /// 조이스틱 기능
    /// </summary>
    /// <param name="joystick">조이스틱 렉트 트랜스폼</param>
    /// <param name="joystickBgRect">조이스틱 배경 이미지 렉트 트랜스폼</param>
    /// <param name="touchPos">사용자가 터치한 위치</param>
    public static void ClampJoyStick(this RectTransform joystick, RectTransform joystickBgRect, Vector2 touchPos)
    {
        joystick.position = CalculatePos(joystickBgRect, touchPos);
    }

    /// <summary>
    /// DOTween을 사용한 조이스틱 기능
    /// </summary>
    /// <param name="joystick">조이스틱 렉트 트랜스폼</param>
    /// <param name="joystickBgRect">조이스틱 배경 이미지 렉트 트랜스폼</param>
    /// <param name="touchPos">사용자가 터치한 위치</param>
    /// <param name="tweenTime">트윈 시간</param>
    public static void DOClampJoyStick(this RectTransform joystick, RectTransform joystickBgRect, Vector2 touchPos, float tweenTime)
    {
        joystick.DOMove(CalculatePos(joystickBgRect, touchPos), tweenTime);
    }
}
