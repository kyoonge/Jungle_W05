using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraPoint : MonoBehaviour
{
    public Transform Camera;
    public Transform targetCameraPoint1;
    public Transform targetCameraPoint2;

    private bool isAtTarget1 = true;

    private void Start()
    {
        // 초기에는 targetCameraPoint1을 타겟으로 설정
        MoveCameraToTarget(targetCameraPoint1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 트리거에 진입할 때마다 다음 타겟으로 이동
        if (isAtTarget1)
        {
            MoveCameraToTarget(targetCameraPoint2);
        }
        else
        {
            MoveCameraToTarget(targetCameraPoint1);
        }

        // 상태 토글
        isAtTarget1 = !isAtTarget1;
    }

    private void MoveCameraToTarget(Transform target)
    {
        Camera.DOMoveX(target.position.x, 3f);
    }
}

