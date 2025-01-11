using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFont : Entity
{
    public float MoveWorldPosY { get; set; }    // Y축 이동거리

    public int HitDamage { get; set; }  // 피격 데미지
    
    // 오버레이 캔버스에 띄우면서 카메라가 움직임에 따라 폰트가 따라오는 것을 방지하기 위함
    public Vector3 InitWorldPositon { get; set; }   // 생성되었을 때, 위치 값
    public Vector3 EndWorldPosition { get; set; }   // 움직이고 나서 최종 위치 값

    public float ArriveDeltaTime { get; set; }  // 생존 시간 데이터 -> 방금 막 소환 기준으로 0
    public override void Init(EntityObject myObject)
    {
        base.Init(myObject);

        ArriveDeltaTime = 0;
    }
}
