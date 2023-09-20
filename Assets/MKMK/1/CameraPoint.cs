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
        // �ʱ⿡�� targetCameraPoint1�� Ÿ������ ����
        MoveCameraToTarget(targetCameraPoint1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���ſ� ������ ������ ���� Ÿ������ �̵�
        if (isAtTarget1)
        {
            MoveCameraToTarget(targetCameraPoint2);
        }
        else
        {
            MoveCameraToTarget(targetCameraPoint1);
        }

        // ���� ���
        isAtTarget1 = !isAtTarget1;
    }

    private void MoveCameraToTarget(Transform target)
    {
        Camera.DOMoveX(target.position.x, 3f);
    }
}

