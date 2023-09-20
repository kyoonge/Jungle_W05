using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Magnet : MonoBehaviour
{
    #region PublicVariables
    [SerializeField] public Enums.MagnetType magnetType;
    public bool isMovable = false;
    

    [Header("MagnetMove")]
    public bool tweenStart = false;
    public Tweener moveTweener;
    private float moveDuration;
    private float playerOffset;
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private int targetDirection;

    [Header("MagnetButton")]
    public bool buttonTweenStart = false;
    public Tweener buttonTweener;
    [SerializeField] private float buttonDuration = 3f;
    public float jumpHeight =3; // ���ϴ� ���� ����
    private float gravity = 9.8f; // �߷� ���ӵ�

    [Header("Raycast")]
    public LayerMask collisionTileLayer;
    // ���̾� ����ũ ���� �� ����
    int layerMask1; // ù ��° ���̾�
    int layerMask2; // �� ��° ���̾�
    int combinedLayerMask; // �� ���̾ ����
    private float boxOffset;
    private Vector2 boxSize;
    private Vector3 collisionPosition;
    private int curDirection;

    [Header("Rigidbody")]
    public Rigidbody2D rb;
    public Rigidbody2D playerRb;
    public float magnetForce = 1f;

    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod

    public Magnet(Enums.MagnetType _magnetType, bool _isMovable)
    {
        this.magnetType = _magnetType;
        this.isMovable = _isMovable;
    }
    private void Start()
    {
        playerOffset = 1f;

        boxOffset = 0.5f;
        boxSize = new Vector2(2 * boxOffset, 3 * boxOffset);

        // ���̾� ����ũ ���� �� ����
        layerMask1 = 1 << LayerMask.NameToLayer("Magnet"); // ù ��° ���̾�
        layerMask2 = 1 << LayerMask.NameToLayer("Ground"); // �� ��° ���̾�
        combinedLayerMask = layerMask1 | layerMask2; // �� ���̾ ����

        playerRb = MagnetManager.Instance.player.GetComponent<Rigidbody2D>();

    }

    public void CallMagnet(PlayerMagnet _playerMagnet)
    {
        moveDuration = _playerMagnet.inputHoldTime;
        if (isMovable)
        {
            movableMagnet(_playerMagnet);
            //�ڽ� �浹 �˻�
            CheckTileorMagnet(_playerMagnet);
        }
       
    }
    public void CallMagnetButton(PlayerMagnet _playerMagnet)
    {
        Debug.Log("callmagnetButton");
        if(_playerMagnet.magnetType == magnetType)
        {
            UnmovableMagnet(_playerMagnet);
        }
        
        //CheckPlayerOnButton(_playerMagnet);
    }

    public void ExitMagnet()
    {
        if(moveTweener != null)
        {
            moveTweener.Kill();
            moveTweener = null;
        }
        //if (buttonTweener != null)
        //{
        //    buttonTweener.Kill();
        //    buttonTweener = null;
        //}
        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = 3;
        buttonTweenStart = false;
        Debug.Log("buttonTweenStart false");
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
        collisionPosition = transform.position + (new Vector3(0.2f, 0f, 0) * curDirection); // �÷��̾��� ��ġ
        Vector3 boxCenter = collisionPosition; // �ڽ��� �߽� ��ġ
        // �ڽ��� ���̾� �浹 �˻� ����
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, combinedLayerMask);

        if (colliders.Length > 0)
        {
            // �浹�� ������Ʈ�� �ִ� ���
            foreach (Collider2D collider in colliders)
            {
                // ���� ����
                if (collider != null && collider.gameObject != this.gameObject)
                {
                    //���ΰ� ��� DoTween ����
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
        // ����׿����� ���� �ڽ��� �׸��� �ڵ� (Scene �信���� ����)
        Gizmos.DrawWireCube(collisionPosition = transform.position + (new Vector3(0.15f, 0f, 0) * curDirection),boxSize); //�ڽ� ������Ʈ��
    }

    private void UnmovableMagnet(PlayerMagnet _playerMagnet)
    {
        playerRb = MagnetManager.Instance.player.GetComponent<Rigidbody2D>();
        Debug.Log("unmovableMagnet");
        if (buttonTweenStart == true)
        {
            if(playerRb.velocity.y <= 0)
            {
                playerRb.gravityScale = 0;
                //transform.DOShakePosition(0.2, 0.1, 0.1);
                Debug.Log("gravity = 0");
            }
            return;
        }
        else
        {
            float jumpTime = Mathf.Sqrt((2 * jumpHeight) / 9.8f);
            // �����ϴ� ����� �� ���
            Vector2 jumpForce = 1 * (Vector2.up * 9.8f) / jumpTime;
            // AddForce�� ����
            Debug.Log("jumpForce: "+jumpForce);
            playerRb.AddForce(jumpForce, ForceMode2D.Impulse);
            //playerRb.AddForce(new Vector2(0,10f), ForceMode2D.Impulse);

            buttonTweenStart = true;

            
        }

        Debug.Log("�ӵ�: " + playerRb.velocity.y + " ����: " + jumpHeight);
    }

    
    


    #endregion
}