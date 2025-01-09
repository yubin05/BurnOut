using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : LocalSingleton<CameraSystem>
{
    [SerializeField] private Camera targetCamera;
    public Camera TargetCamera => targetCamera;
    private float targetCameraZ;

    public bool IsTracking { get; private set; }
    public CharacterObject TargetObject { get; private set; }

    public void Init()
    {
        targetCameraZ = TargetCamera.transform.position.z;
        IsTracking = false;
        TargetObject = null;
    }

    private void Update()
    {
        if (!IsTracking) return;

        if (TargetObject != null)
        {
            var a = new Vector3(TargetCamera.transform.position.x, TargetCamera.transform.position.y, targetCameraZ);
            var b = new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, targetCameraZ);
            TargetCamera.transform.position = Vector3.Lerp(a, b, Time.deltaTime);
        }
    }

    // 카메라 트래킹을 시작합니다.
    public void StartTracking(CharacterObject targetObject)
    {
        IsTracking = true;
        SetTargetObject(targetObject);
    }

    // 카메라 트래킹을 멈춥니다.
    public void StopTracking()
    {
        IsTracking = false;
        SetTargetObject(null);
    }

    // 추적 대상을 설정합니다.
    public void SetTargetObject(CharacterObject targetObject)
    {
        TargetObject = targetObject;
    }
}
