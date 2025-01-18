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
    
    private Vector2 minValues;  // 카메라 최소 범위
    private Vector2 maxValues;  // 카메라 최대 범위

    private void Awake()
    {
        targetCameraZ = TargetCamera.transform.position.z;
    }

    private void LateUpdate()
    {
        if (!IsTracking) return;

        if (TargetObject != null)
        {
            var a = new Vector3(TargetCamera.transform.position.x, TargetCamera.transform.position.y, targetCameraZ);
            var b = new Vector3(TargetObject.transform.position.x, TargetObject.transform.position.y, targetCameraZ);
            TargetCamera.transform.position = AdjustCamera(Vector3.Lerp(a, b, Time.deltaTime));
        }
    }

    /// <summary>
    /// 카메라 트래킹을 시작합니다.
    /// </summary>
    /// <param name="stageId"></param>
    /// <param name="targetObject"></param>
    /// <param name="forceMove"></param>
    public void StartTracking(int stageId, CharacterObject targetObject, bool forceMove=false)
    {
        // 스테이지에 따른 카메라 최소, 최대 범위 초기화
        var stageInfo = GameApplication.Instance.GameModel.PresetData.ReturnData<StageInfo>(nameof(StageInfo), stageId);
        minValues = new Vector2(stageInfo.CameraMinX, stageInfo.CameraMinY);
        maxValues = new Vector2(stageInfo.CameraMaxX, stageInfo.CameraMaxY);

        IsTracking = true;
        SetTargetObject(targetObject);
        if (forceMove) TargetCamera.transform.position = AdjustCamera(targetObject.transform.position);
    }

    /// <summary>
    /// 카메라 트래킹을 멈춥니다.
    /// </summary>
    public void StopTracking()
    {
        IsTracking = false;
        SetTargetObject(null);
    }

    /// <summary>
    /// 추적 대상을 설정합니다.
    /// </summary>
    /// <param name="targetObject"></param>
    public void SetTargetObject(CharacterObject targetObject)
    {
        TargetObject = targetObject;
    }

    /// <summary>
    /// 카메라 최소, 최대 범위 밖으로 벗어나지 않도록 보정합니다.
    /// </summary>
    /// <param name="cameraPos"></param>
    /// <returns></returns>
    private Vector3 AdjustCamera(Vector3 cameraPos)
    {
        var adjustPos = cameraPos;
        adjustPos.x = Mathf.Clamp(adjustPos.x, minValues.x, maxValues.x);
        adjustPos.y = Mathf.Clamp(adjustPos.y, minValues.y, maxValues.y);
        return adjustPos;
    }
}
