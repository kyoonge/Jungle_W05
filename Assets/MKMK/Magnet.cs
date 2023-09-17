using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Magnet : MonoBehaviour
{
    #region PublicVariables
    [SerializeField] public Utils.MagnetType magnetType;
    public bool isMovable = false;
    

    [Header("MagnetMove")]
    public bool tweenStart = false;
    public Tweener moveTweener;
    private float moveDuration;
    private float playerOffset;
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private int targetDirection;

    [Header("Raycast")]
    public LayerMask collisionTileLayer;
    // 레이어 마스크 생성 및 결합
    int layerMask1; // 첫 번째 레이어
    int layerMask2; // 두 번째 레이어
    int combinedLayerMask; // 두 레이어를 결합
    private float boxOffset;
    private Vector2 boxSize;
    private Vector3 collisionPosition;
    private int curDirection;

    [Header("Rigidbody")]
    public Rigidbody2D rb;
    public float magnetForce = 10f;

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
        playerOffset = 1f;

        boxOffset = 0.5f;
        boxSize = new Vector2(2 * boxOffset, 4 * boxOffset);

        // 레이어 마스크 생성 및 결합
        layerMask1 = 1 << LayerMask.NameToLayer("Magnet"); // 첫 번째 레이어
        layerMask2 = 1 << LayerMask.NameToLayer("Ground"); // 두 번째 레이어
        combinedLayerMask = layerMask1 | layerMask2; // 두 레이어를 결합

        

    }
    public void CallMagnet(PlayerMagnet _playerMagnet)
    {
        moveDuration = _playerMagnet.inputHoldTime;
        if (isMovable)
        {
            movableMagnet(_playerMagnet);
            //박스 충돌 검사
            CheckTileorMagnet(_playerMagnet);
        }
        else
        {
            unmovableMagnet(_playerMagnet);
        }
    }

    public void ExitMagnet()
    {
        if(moveTweener != null)
        {
            moveTweener.Kill();
            moveTweener = null;
        }
        tweenStart = false;
        collisionPosition = transform.position + (new Vector3(0.15f, 0, 0) * curDirection);
    }
    #endregion
    #region PrivateMethod


    private void movableMagnet(PlayerMagnet _playerMagnet)
    {
        if (tweenStart == true)
            return;

        targetDirection = Mathf.Sign(_playerMagnet.transform.position.x - transform.position.x) < 0 ? 1 : -1;
        
        if (_playerMagnet.magnetType == magnetType)
        {
            targetPosition = new Vector3(targetDirection * _playerMagnet.boxOffset + _playerMagnet.transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            targetPosition = new Vector3(targetDirection * playerOffset + _playerMagnet.transform.position.x, transform.position.y, transform.position.z);
        }
        if (tweenStart == false)
        {
            tweenStart = true;
            moveTweener = transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear);
        }
        if (moveTweener != null)
        {
            moveTweener.ChangeEndValue(targetPosition, true).Restart();
        }

        curDirection = Mathf.Sign(targetPosition.x - transform.position.x) > 0? 1:-1;
    }

    private void CheckTileorMagnet(PlayerMagnet _playerMagnet)
    {
        //float closestDistance = 10f;
        
        
        collisionPosition = transform.position + (new Vector3(0.15f, 0.15f, 0) * curDirection); // 플레이어의 위치
        Vector3 boxCenter = collisionPosition; // 박스의 중심 위치
        // 박스와 레이어 충돌 검사 수행
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, combinedLayerMask);

        if (colliders.Length > 0)
        {
            // 충돌한 오브젝트가 있는 경우
            foreach (Collider2D collider in colliders)
            {
                if (collider != null && collider.gameObject != this.gameObject)
                {
                    Debug.Log(this.gameObject +"  "+collider.gameObject);
                    moveTweener.Kill();
                    _playerMagnet.MagnetCanceled();
                    if(collider.gameObject.layer == LayerMask.NameToLayer("Magnet"))
                    {
                        collider.gameObject.GetComponent<Magnet>().ExitMagnet();
                    }
                    
                    return;
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // 디버그용으로 검출 박스를 그리는 코드 (Scene 뷰에서만 보임)
        Gizmos.DrawWireCube(collisionPosition = transform.position + (new Vector3(0.15f, 0.15f, 0) * curDirection),boxSize); // 플레이어의 위치, boxSize);
    }

    private void unmovableMagnet(PlayerMagnet _playerMagnet)
    {
        Debug.Log("magnet jump");
        rb = _playerMagnet.transform.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * magnetForce, ForceMode2D.Impulse);
    }
    #endregion
}