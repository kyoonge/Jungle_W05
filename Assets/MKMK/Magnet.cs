using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Magnet : MonoBehaviour
{
    #region PublicVariables
    [SerializeField] public Utils.MagnetType magnetType;
    public bool isMovable = false;
    public bool isWork = true;

    [Header("MagnetMove")]
    [SerializeField] public Tween moveTween;
    //public Tweener MoveTween;
    private float moveDuration;
    private float playerOffset;
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private int targetDirection;

    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod

    public Magnet(Utils.MagnetType _magnetType, bool _isMovable)
    {
        this.magnetType = _magnetType;
        this.isMovable = _isMovable;
    }
    private void Start()
    {
        //targetPosition = MagnetManager.Instance.player.position;
        moveDuration = 1f;
        playerOffset = 1f;
    }
    public void CallMagnet(PlayerMagnet _playerMagnet)
    {
        Debug.Log("It's Magnet Object");

        if (moveTween != null)
        {
            moveTween.Kill();
        }

        targetDirection = Mathf.Sign(_playerMagnet.transform.position.x - transform.position.x) < 0 ? 1 : -1;

        
        if (_playerMagnet.magnetType == magnetType)
        {
            Debug.Log("1 " + _playerMagnet.magnetType+ magnetType);
            targetPosition = new Vector3(targetDirection * _playerMagnet.boxOffset + _playerMagnet.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            Debug.Log("2 " + _playerMagnet.magnetType + magnetType);
            targetPosition = new Vector3(targetDirection * playerOffset + _playerMagnet.transform.position.x, transform.position.y, transform.position.z);
        }
        Debug.Log(targetPosition);
        moveTween = transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // 이동이 완료된 후 실행할 코드
                        Debug.Log("Player reached Magnet");

                        _playerMagnet.MagnetCanceled();
                    });
    }

    public void ExitMagnet()
    {
        moveTween.Kill();
        moveTween = null;
    }
    #endregion
    #region PrivateMethod


    private void getDirection()
    {

    }

    #endregion
}